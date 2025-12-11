namespace BizCompany.API.Entities
{
    public class Blog : IEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? ContentImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public int? WriterId { get; set; } //Delete Behavior için nullable yaptım
        public Writer? Writer { get; set; }
        public int? CategoryId { get; set; } //Delete Behavior için nullable yaptım
        public BlogCategory? Category { get; set; }
        public ICollection<BlogTag>? BlogTags { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
