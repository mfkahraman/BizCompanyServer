using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogTagsController(IBaseRepository<BlogTag> repository)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await repository.GetAllAsync();
            if (values == null || values.Count == 0)
            {
                return BadRequest("No records found.");
            }

            var records = values.Select(x => new BlogTagDto
            {
                BlogId = x.BlogId,
                TagId = x.TagId
            });

            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await repository.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"Record with ID {id} not found.");
            }

            var dto = new BlogTagDto
            {
                BlogId = value.BlogId,
                TagId = value.TagId
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogTagDto dto)
        {
            try
            {
                if (!dto.BlogId.HasValue)
                {
                    return BadRequest("BlogId is required.");
                }

                var blogTag = new BlogTag
                {
                    BlogId = dto.BlogId.Value,
                    TagId = dto.TagId
                };

                var result = await repository.CreateAsync(blogTag);
                if (!result)
                {
                    return BadRequest("Failed to create record.");
                }

                return CreatedAtAction(nameof(GetById), new { id = blogTag.Id }, blogTag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BlogTagDto dto)
        {
            try
            {
                if (!dto.BlogId.HasValue)
                {
                    return BadRequest("BlogId is required.");
                }

                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                record.BlogId = dto.BlogId.Value;
                record.TagId = dto.TagId;

                var result = await repository.UpdateAsync(record);

                if (!result)
                    return BadRequest("An error occurred while updating the record");

                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred during the update operation: {ex.Message}");
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
                    return BadRequest($"An error occurred while deleting the record with ID {id}.");
                }

                return Ok($"Record with ID {id} successfully deleted.");
            }

            catch (Exception ex)
            {
                return BadRequest($"An error occurred during the delete operation: {ex.Message}");
            }
        }
    }
}
