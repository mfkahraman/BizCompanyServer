using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialsController(IBaseRepository<Testimonial> repository)
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

            var records = values.Select(x => new GetTestimonialDto
            {
                Id = x.Id,
                ClientName = x.ClientName,
                Title = x.Title,
                Comment = x.Comment,
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
                return NotFound($"{id} nolu ID ile bir kayıt bulunamadı");
            }

            var dto = new GetTestimonialDto
            {
                Id = value.Id,
                ClientName = value.ClientName,
                Title = value.Title,
                Comment = value.Comment,
                ImageUrl = value.ImageUrl
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TestimonialDto dto)
        {
            try
            {
                var value = new Testimonial
                {
                    ClientName = dto.ClientName,
                    Title = dto.Title,
                    Comment = dto.Comment,
                    ImageUrl = dto.ImageUrl
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
        public async Task<IActionResult> Update(int id, TestimonialDto dto)
        {
            try
            {
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"{id} Id nolu kayıt bulunamadı.");
                }

                record.ClientName = dto.ClientName;
                record.Title = dto.Title;
                record.Comment = dto.Comment;
                record.ImageUrl = dto.ImageUrl;

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