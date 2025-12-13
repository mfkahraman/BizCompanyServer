using BizCompany.API.Entities;

namespace BizCompany.API.DTOs
{
    public class GetCommentsDto
    {
        public int Id { get; set; }
        public int? BlogId { get; set; }
        public int? WriterId { get; set; }
        public GetWriterDto? Writer { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
