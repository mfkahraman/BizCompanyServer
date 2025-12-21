using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

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
            if (values == null || values.Count == 0)
            {
                return NotFound("No blogs found.");
            }

            try
            {
                var blogs = values.Select(values => new GetBlogDto
                {
                    Id = values.Id,
                    Title = values.Title,
                    Content = values.Content,
                    CoverImageUrl = values.CoverImageUrl,
                    ContentImageUrl = values.ContentImageUrl,
                    CreatedAt = values.CreatedAt,
                    WriterId = values.WriterId,
                    CategoryId = values.CategoryId
                }).ToList();

                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Bloglar DTO'lara dönüştürülürken hata oldu: {ex.Message}");
            }
        }

        [HttpGet("get-blogs-with-details")]
        public async Task<IActionResult> GetBlogsWithDetails()
        {
            var values = await blogRepository.GetBlogsWithDetailsAsync();
            if (values == null || values.Count == 0)
            {
                return NotFound("No blogs with details found.");
            }

            var blogs = values.Select(values => new GetBlogWithDetailsDto
            {
                id = values.Id,
                title = values.Title,
                content = values.Content,
                coverImageUrl = values.CoverImageUrl,
                contentImageUrl = values.ContentImageUrl,
                createdAt = values.CreatedAt,
                writerId = values.WriterId,
                writer = values.Writer != null ? new GetWriterDto
                {
                    Id = values.Writer.Id,
                    FullName = values.Writer.FullName,
                    Bio = values.Writer.Bio,
                    ImageUrl = values.Writer.ImageUrl
                } : null,
                categoryId = values.CategoryId,
                category = values.Category != null ? new GetBlogCategoryDto
                {
                    Id = values.Category.Id,
                    Name = values.Category.Name
                } : null,
                blogTags = values.BlogTags?.Select(bt => new GetBlogTagDto
                {
                    Id = bt.Id,
                    BlogId = bt.BlogId,
                    TagId = bt.TagId,
                    Tags = bt.Tag != null
                        ? new List<GetTagDto>
                        {
                            new GetTagDto
                            {
                                Id = bt.Tag.Id,
                                Name = bt.Tag.Name
                            }
                        }
                        : new List<GetTagDto>()
                }).ToList(),
                comments = values.Comments?.Select(c => new GetCommentsDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    WriterId = c.WriterId,
                    CreatedAt = c.CreatedAt
                }).ToList()
            }).ToList();

            return Ok(blogs);
        }

        [HttpGet("get-blogs-with-details-by-id/{id}")]
        public async Task<IActionResult> GetBlogsWithDetailsById(int id)
        {
            var value = await blogRepository.GetBlogWithDetailsByIdAsync(id);
            if (value == null)
            {
                return NotFound($"Blog with ID {id} not found.");
            }

            var blog = new GetBlogWithDetailsDto
            {
                id = value.Id,
                title = value.Title,
                content = value.Content,
                coverImageUrl = value.CoverImageUrl,
                contentImageUrl = value.ContentImageUrl,
                createdAt = value.CreatedAt,
                writerId = value.WriterId,
                writer = value.Writer != null ? new GetWriterDto
                {
                    Id = value.Writer.Id,
                    FullName = value.Writer.FullName,
                    Bio = value.Writer.Bio,
                    ImageUrl = value.Writer.ImageUrl
                } : null,
                categoryId = value.CategoryId,
                category = value.Category != null ? new GetBlogCategoryDto
                {
                    Id = value.Category.Id,
                    Name = value.Category.Name
                } : null,
                blogTags = value.BlogTags?.Select(bt => new GetBlogTagDto
                {
                    Id = bt.Id,
                    BlogId = bt.BlogId,
                    TagId = bt.TagId,
                    Tags = bt.Tag != null
                        ? new List<GetTagDto>
                        {
                            new GetTagDto
                            {
                                Id = bt.Tag.Id,
                                Name = bt.Tag.Name
                            }
                        }
                        : new List<GetTagDto>()
                }).ToList(),
                comments = value.Comments?.Select(c => new GetCommentsDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    WriterId = c.WriterId,
                    Writer = c.Writer != null ? new GetWriterDto
                    {
                        Id = c.Writer.Id,
                        FullName = c.Writer.FullName,
                        Bio = c.Writer.Bio,
                        ImageUrl = c.Writer.ImageUrl
                    } : null,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };
            return Ok(blog);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var value = await repository.GetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"Blog with ID {id} not found.");
            }
            var blogDto = new GetBlogDto
            {
                Id = value.Id,
                Title = value.Title,
                Content = value.Content,
                CoverImageUrl = value.CoverImageUrl,
                ContentImageUrl = value.ContentImageUrl,
                CreatedAt = value.CreatedAt,
                WriterId = value.WriterId,
                CategoryId = value.CategoryId
            };
            return Ok(blogDto);
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
                    BlogTags = dto.BlogTags?.Select(bt => new BlogTag
                    {
                        TagId = bt.TagId
                    }).ToList()
                };
                var result = await blogRepository.CreateBlogAsync(blog);
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
                    BlogTags = dto.BlogTags?.Select(bt => new BlogTag
                    {
                        TagId = bt.TagId
                    }).ToList()
                };

                var result = await blogRepository.UpdateBlogAsync(blog);

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
