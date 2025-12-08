namespace BizCompany.API.Entities
{
    public class BlogImage
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public Blog Blog { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
