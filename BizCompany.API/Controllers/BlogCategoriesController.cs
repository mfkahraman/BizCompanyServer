using BizCompany.API.DataAccess;
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
                return NotFound("No blogs found.");
            }
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await repository.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"Blog with ID {id} not found.");
            }
            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogCategory value)
        {
            try
            {
                var result = repository.CreateAsync(value);
                if (result == null)
                {
                    return BadRequest("Failed to create blog.");
                }

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BlogCategory value)
        {
            if (id != value.Id)
                return BadRequest("URL'deki id ile gövdedeki id uyuşmuyor.");

            try
            {
                var result = await repository.UpdateAsync(value);

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
