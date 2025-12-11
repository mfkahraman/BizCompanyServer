namespace BizCompany.API.DTOs
{
    public class GetCommentDto
    {
        public int Id { get; set; }
        public int? BlogId { get; set; }
        public int? WriterId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
