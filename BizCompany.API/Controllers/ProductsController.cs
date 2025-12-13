using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IBaseRepository<Product> repository,
                                    ProductRepository productRepository)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await repository.GetAllAsync();
            if (values == null || values.Count == 0)
            {
                return BadRequest("Hiç kayıt bulunamadı.");
            }

            var records = values.Select(x => new GetProductDto
            {
                Id = x.Id,
                ProductName = x.ProductName,
                Description = x.Description,
                ImagePath = x.ImagePath,
                CategoryId = x.CategoryId
            }).ToList();

            return Ok(records);
        }

        [HttpGet("get-products-with-details")]
        public async Task<IActionResult> GetProductsWithDetails()
        {
            var values = await productRepository.GetProductsWithDetailAsync();
            if (values == null || values.Count == 0)
            {
                return NotFound("Detaylı ürün kaydı bulunamadı.");
            }
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await repository.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"{id} nolu ID ile bir kayıt bulunamadı");
            }

            var dto = new GetProductDto
            {
                Id = value.Id,
                ProductName = value.ProductName,
                Description = value.Description,
                ImagePath = value.ImagePath,
                CategoryId = value.CategoryId
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDto dto)
        {
            try
            {
                var value = new Product
                {
                    ProductName = dto.ProductName!,
                    Description = dto.Description,
                    ImagePath = dto.ImagePath,
                    CategoryId = dto.CategoryId
                };

                var result = await repository.CreateAsync(value);
                if (!result)
                {
                    return BadRequest("Kayıt oluşturulamadı.");
                }

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDto dto)
        {
            try
            {
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"{id} Id nolu kayıt bulunamadı.");
                }

                record.ProductName = dto.ProductName!;
                record.Description = dto.Description;
                record.ImagePath = dto.ImagePath;
                record.CategoryId = dto.CategoryId;

                var result = await repository.UpdateAsync(record);

                if (!result)
                    return BadRequest("Kayıt güncellenirken bir sorun oluştu");

                return Ok("Başarılı şekilde güncellendi");
            }
            catch (Exception ex)
            {
                return BadRequest($"Güncelleme işlemi sırasında bir sorun oluştu: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await repository.RemoveAsync(id);

                if (!result)
                {
                    return BadRequest($"{id} Id nolu kayıt silinirken bir hata oluştu.");
                }

                return Ok($"{id} Id nolu kayıt başarıyla silindi.");
            }

            catch (Exception ex)
            {
                return BadRequest($"Silme işlemi sırasında bir sorun oluştu: {ex.Message}");
            }
        }
    }
}
