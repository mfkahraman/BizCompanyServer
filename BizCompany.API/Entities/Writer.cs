namespace BizCompany.API.Entities
{
    public class Writer
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Blog>? Blogs { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
