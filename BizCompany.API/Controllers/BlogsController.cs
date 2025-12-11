using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController(IBaseRepository<Blog> repository,
                                 BlogRepository blogRepository)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await repository.GetAllAsync();
            if (values == null || !values.Any())
            {
                return NotFound("No blogs found.");
            }
            return Ok(values);
        }

        [HttpGet("get-blogs-with-details")]
        public async Task<IActionResult> GetBlogsWithDetails()
        {
            var values = await blogRepository.GetBlogsWithDetailsAsync();
            if (values == null || !values.Any())
            {
                return NotFound("No blogs with details found.");
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
        public async Task<IActionResult> Create(BlogDto dto)
        {
            try
            {
                Blog blog = new()
                {
                    Title = dto.Title,
                    Content = dto.Content,
                    CoverImageUrl = dto.CoverImageUrl,
                    ContentImageUrl = dto.ContentImageUrl,
                    WriterId = dto.WriterId,
                    CategoryId = dto.CategoryId,
                };
                var result = await repository.CreateAsync(blog);
                if (!result)
                {
                    return BadRequest("Failed to create blog.");
                }

                return CreatedAtAction(nameof(GetById), new { id = blog.Id }, blog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BlogDto dto)
        {
            try
            {
                Blog blog = new()
                {
                    Id = id,
                    Title = dto.Title,
                    Content = dto.Content,
                    CoverImageUrl = dto.CoverImageUrl,
                    ContentImageUrl = dto.ContentImageUrl,
                    WriterId = dto.WriterId,
                    CategoryId = dto.CategoryId,
                };

                var result = await repository.UpdateAsync(blog);

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
