using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WritersController(IBaseRepository<Writer> repository,
                                   IImageService imageService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await repository.GetAllAsync();
            if (values == null || values.Count == 0)
                return NotFound("No records found.");

            var records = values.Select(x => new GetWriterDto
            {
                Id = x.Id,
                FullName = x.FullName,
                Bio = x.Bio,
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

            var dto = new GetWriterDto
            {
                Id = value.Id,
                FullName = value.FullName,
                Bio = value.Bio,
                ImageUrl = value.ImageUrl
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] WriterDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            string? savedImagePath = null;

            try
            {
                if (dto.ImageFile != null)
                {
                    var validationError = ValidateImageFile(dto.ImageFile);
                    if (validationError != null) return validationError;

                    savedImagePath = await imageService.SaveImageAsync(dto.ImageFile, "writers");
                    dto.ImageUrl = savedImagePath;
                }

                var value = new Writer
                {
                    FullName = dto.FullName,
                    Bio = dto.Bio,
                    ImageUrl = dto.ImageUrl
                };

                var result = await repository.CreateAsync(value);
                if (!result)
                {
                    if (savedImagePath != null)
                        await imageService.DeleteImageAsync(savedImagePath);

                    return BadRequest("Record could not be created.");
                }

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                if (savedImagePath != null)
                    await imageService.DeleteImageAsync(savedImagePath);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] WriterDto dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            string? newImagePath = null;

            try
            {
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                var oldImagePath = record.ImageUrl;

                if (dto.ImageFile != null)
                {
                    var validationError = ValidateImageFile(dto.ImageFile);
                    if (validationError != null) return validationError;

                    newImagePath = await imageService.SaveImageAsync(dto.ImageFile, "writers");
                }

                record.FullName = dto.FullName;
                record.Bio = dto.Bio;
                if (newImagePath != null)
                    record.ImageUrl = newImagePath; ;

                var result = await repository.UpdateAsync(record);

                if (!result)
                {
                    if (newImagePath != null)
                        await imageService.DeleteImageAsync(newImagePath);

                    return BadRequest("An error occurred while updating the record");
                }

                if (newImagePath != null && !string.IsNullOrEmpty(oldImagePath))
                    await imageService.DeleteImageAsync(oldImagePath);


                return Ok("Successfully updated");
            }
            catch (Exception ex)
            {
                if (newImagePath != null)
                    await imageService.DeleteImageAsync(newImagePath);

                return BadRequest($"An error occurred during the update operation: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                if (!string.IsNullOrEmpty(record.ImageUrl))
                    await imageService.DeleteImageAsync(record.ImageUrl);

                var result = await repository.RemoveAsync(id);

                if (!result)
                {
                    return StatusCode(500, $"An error occurred while deleting the record with ID {id}.");
                }

                return Ok($"Record with ID {id} successfully deleted.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred during the delete operation: {ex.Message}");
            }
        }

        private BadRequestObjectResult? ValidateImageFile(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                return BadRequest("Invalid file type. Only images are allowed.");

            if (file.Length > 5 * 1024 * 1024) // 5MB
                return BadRequest("File size cannot exceed 5MB.");

            return null; // Valid
        }
    }
}