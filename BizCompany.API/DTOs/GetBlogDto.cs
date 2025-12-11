namespace BizCompany.API.DTOs
{
    public class GetBlogDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? ContentImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? WriterId { get; set; }
        public int? CategoryId { get; set; }
    }
}
