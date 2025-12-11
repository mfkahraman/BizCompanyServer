using BizCompany.API.DataAccess;
using BizCompany.API.DTOs;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController(IBaseRepository<Message> repository)
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

            var records = values.Select(x => new GetMessageDto
            {
                Id = x.Id,
                SenderEmail = x.SenderEmail,
                SenderName = x.SenderName,
                Subject = x.Subject,
                Content = x.Content,
                SentAt = x.SentAt,
                IsRead = x.IsRead
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

            var dto = new GetMessageDto
            {
                Id = value.Id,
                SenderEmail = value.SenderEmail,
                SenderName = value.SenderName,
                Subject = value.Subject,
                Content = value.Content,
                SentAt = value.SentAt,
                IsRead = value.IsRead
            };

            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(MessageDto dto)
        {
            try
            {
                var value = new Message
                {
                    SenderName = dto.SenderName,
                    SenderEmail = dto.SenderEmail,
                    Subject = dto.Subject,
                    Content = dto.Content,
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
        public async Task<IActionResult> Update(int id, MessageDto dto)
        {
            try
            {
                var record = await repository.GetByIdAsync(id);
                if (record == null)
                {
                    return NotFound($"{id} Id nolu kayıt bulunamadı.");
                }

                record.SenderName = dto.SenderName;
                record.SenderEmail = dto.SenderEmail;
                record.Subject = dto.Subject;
                record.Content = dto.Content;

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