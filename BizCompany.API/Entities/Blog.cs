namespace BizCompany.API.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public int WriterId { get; set; }
        public Writer? Writer { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<BlogTag>? BlogTags { get; set; }
    }
}
