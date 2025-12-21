using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoriesController(IBaseRepository<ProductCategory> repository)
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

            var records = values.Select(x => new GetCategoryDto
            {
                Id = x.Id,
                CategoryName = x.CategoryName
            }).ToList();

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

            var dto = new GetCategoryDto
            {
                Id = value.Id,
                CategoryName = value.CategoryName
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCategoryDto dto)
        {
            try
            {
                var value = new ProductCategory
                {
                    CategoryName = dto.CategoryName
                };

                var result = await repository.CreateAsync(value);
                if (!result)
                {
                    return BadRequest("Failed to create record.");
                }

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductCategoryDto dto)
        {
            try
            {
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                record.CategoryName = dto.CategoryName;

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
