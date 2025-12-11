namespace BizCompany.API.DTOs
{
    public class CommentDto
    {
        public int? BlogId { get; set; }
        public int? WriterId { get; set; }
        public string? Content { get; set; }
    }
}
