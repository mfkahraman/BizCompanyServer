namespace BizCompany.API.DTOs
{
    public class GetBlogWithDetailsDto
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? content { get; set; }
        public string? coverImageUrl { get; set; }
        public string? contentImageUrl { get; set; }
        public DateTime createdAt { get; set; }
        public int? writerId { get; set; }
        public GetWriterDto? writer { get; set; }
        public int? categoryId { get; set; }
        public GetBlogCategoryDto? category { get; set; }
        public List<GetBlogTagDto>? blogTags { get; set; }
        public List<GetCommentsDto>? comments { get; set; }
    }
}
