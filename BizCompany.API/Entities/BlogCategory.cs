namespace BizCompany.API.Entities
{
    public class BlogCategory
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<Blog>? Blogs { get; set; }
    }
}
