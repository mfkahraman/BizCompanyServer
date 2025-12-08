namespace BizCompany.API.Entities
{
    public class Blog
    {
        public int Id { get; set; }
        public int WriterId { get; set; }
        public Writer? Writer { get; set; }
        public int CategoryId { get; set; }
        public BlogCategory? Category { get; set; }
        public string? Title { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<BlogTag>? BlogTags { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<BlogParagraph>? BlogParagraphs { get; set; }
        public ICollection<BlogImage>? BlogImages { get; set; }

    }
}
