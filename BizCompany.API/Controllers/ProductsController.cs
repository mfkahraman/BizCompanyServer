using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IBaseRepository<Product> repository,
                                    ProductRepository productRepository,
                                    IImageService imageService)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await repository.GetAllAsync();
            if (values == null || values.Count == 0)
                return NotFound("No records found.");

            var records = values.Select(x => new GetProductDto
            {
                Id = x.Id,
                ProductName = x.ProductName!,
                ShortDescription = x.ShortDescription,
                Description = x.Description,
                ImagePath = x.ImagePath,
                ThumbnailImagePath = x.ThumbnailImagePath,
                CategoryId = x.CategoryId,
                ClientName = x.ClientName,
                ProjectUrl = x.ProjectUrl,
                ProjectDate = x.ProjectDate
            }).ToList();

            return Ok(records);
        }

        [HttpGet("get-products-with-details")]
        public async Task<IActionResult> GetProductsWithDetails()
        {
            var values = await productRepository.GetProductsWithDetailAsync();
            if (values == null || values.Count == 0)
            {
                return NotFound("No detailed product records found.");
            }
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await repository.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"No record found with ID {id}");
            }

            var dto = new GetProductDto
            {
                Id = value.Id,
                ProductName = value.ProductName!,
                ShortDescription = value.ShortDescription,
                Description = value.Description,
                ImagePath = value.ImagePath,
                ThumbnailImagePath = value.ThumbnailImagePath,
                CategoryId = value.CategoryId,
                ClientName = value.ClientName,
                ProjectUrl = value.ProjectUrl,
                ProjectDate = value.ProjectDate
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductDto dto)
        {
            string? savedThumbnailPath = null;
            string? savedHeroPath = null;

            try
            {
                if (dto.ThumbnailImageFile != null)
                {
                    var validationError = ValidateImageFile(dto.ThumbnailImageFile);
                    if (validationError != null) return validationError;

                    savedThumbnailPath = await imageService.SaveImageAsync(
                        dto.ThumbnailImageFile, "products");
                    dto.ThumbnailImagePath = savedThumbnailPath;
                }

                if (dto.HeroImageFile != null)
                {
                    var validationError = ValidateImageFile(dto.HeroImageFile);
                    if (validationError != null) return validationError;

                    savedHeroPath = await imageService.SaveImageAsync(
                        dto.HeroImageFile, "products");
                    dto.ImagePath = savedHeroPath;
                }

                var value = new Product
                {
                    ProductName = dto.ProductName!,
                    ShortDescription = dto.ShortDescription,
                    Description = dto.Description,
                    CategoryId = dto.CategoryId,
                    ClientName = dto.ClientName,
                    ProjectUrl = dto.ProjectUrl,
                    ProjectDate = dto.ProjectDate,
                    ImagePath = dto.ImagePath,
                    ThumbnailImagePath = dto.ThumbnailImagePath
                };

                var result = await repository.CreateAsync(value);
                if (!result)
                {
                    if (savedThumbnailPath != null)
                        await imageService.DeleteImageAsync(savedThumbnailPath);
                    if (savedHeroPath != null)
                        await imageService.DeleteImageAsync(savedHeroPath);

                    return BadRequest("Record could not be created.");
                }

                return CreatedAtAction(nameof(GetById), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                if (savedThumbnailPath != null)
                    await imageService.DeleteImageAsync(savedThumbnailPath);
                if (savedHeroPath != null)
                    await imageService.DeleteImageAsync(savedHeroPath);

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] ProductDto dto)
        {
            try
            {
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"Record with ID {id} not found.");
                }

                var oldThumbnailPath = record.ThumbnailImagePath;
                var oldImagePath = record.ImagePath;

                if (dto.ThumbnailImageFile != null)
                {
                    var validationError = ValidateImageFile(dto.ThumbnailImageFile);
                    if (validationError != null) return validationError;

                    if (!string.IsNullOrEmpty(oldThumbnailPath))
                        await imageService.DeleteImageAsync(oldThumbnailPath);

                    var imagePath = await imageService.SaveImageAsync(dto.ThumbnailImageFile, "products");
                    dto.ThumbnailImagePath = imagePath;
                }

                if (dto.HeroImageFile != null)
                {
                    var validationError = ValidateImageFile(dto.HeroImageFile);
                    if (validationError != null) return validationError;

                    if (!string.IsNullOrEmpty(oldImagePath))
                        await imageService.DeleteImageAsync(oldImagePath);

                    var imagePath = await imageService.SaveImageAsync(dto.HeroImageFile, "products");
                    dto.ImagePath = imagePath;
                }

                record.ProductName = dto.ProductName!;
                record.ShortDescription = dto.ShortDescription;
                record.Description = dto.Description;
                record.CategoryId = dto.CategoryId;
                record.ClientName = dto.ClientName;
                record.ProjectUrl = dto.ProjectUrl;
                record.ProjectDate = dto.ProjectDate;

                if (dto.ThumbnailImageFile != null)
                    record.ThumbnailImagePath = dto.ThumbnailImagePath;
                if (dto.HeroImageFile != null)
                    record.ImagePath = dto.ImagePath;

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
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return BadRequest($"Record with ID {id} not found.");
                }

                if (!string.IsNullOrEmpty(record.ThumbnailImagePath))
                    await imageService.DeleteImageAsync(record.ThumbnailImagePath);

                if (!string.IsNullOrEmpty(record.ImagePath))
                    await imageService.DeleteImageAsync(record.ImagePath);

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
