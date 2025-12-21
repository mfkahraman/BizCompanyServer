using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController(IBaseRepository<Employee> repository)
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

            var records = values.Select(x => new GetEmployeeDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Title = x.Title,
                ImageUrl = x.ImageUrl
            }).ToList();

            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await repository.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"No record found with ID {id}");
            }

            var dto = new GetEmployeeDto
            {
                Id = value.Id,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Title = value.Title,
                ImageUrl = value.ImageUrl
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto dto)
        {
            try
            {
                var value = new Employee
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Title = dto.Title,
                    ImageUrl = dto.ImageUrl
                };

                var result = await repository.CreateAsync(value);
                if (!result)
                {
                    return BadRequest("Record could not be created.");
                }

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EmployeeDto dto)
        {
            try
            {
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                record.FirstName = dto.FirstName;
                record.LastName = dto.LastName;
                record.Title = dto.Title;
                record.ImageUrl = dto.ImageUrl;

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