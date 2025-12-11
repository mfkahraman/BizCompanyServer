using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogCategoriesController(IBaseRepository<BlogCategory> repository)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await repository.GetAllAsync();
            if (values == null || values.Count == 0)
            {
                return NotFound("No records found.");
            }
            
            var categories = values.Select(c => new GetBlogCategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });

            return Ok(categories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await repository.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"Record with ID {id} not found.");
            }

            var categoryDto = new GetBlogCategoryDto
            {
                Id = value.Id,
                Name = value.Name
            };
            return Ok(categoryDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogCategoryDto value)
        {
            try
            {
                var entity = new BlogCategory
                {
                    Name = value.Name
                };

                var result = await repository.CreateAsync(entity);
                if (!result)
                {
                    return BadRequest("Failed to create record.");
                }

                return CreatedAtAction(nameof(GetById), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BlogCategoryDto value)
        {
            try
            {
                var entity = new BlogCategory
                {
                    Id = id,
                    Name = value.Name
                };
                var result = await repository.UpdateAsync(entity);

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
