namespace BizCompany.API.Entities
{
    public class Tag : IEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<BlogTag>? BlogTags { get; set; }
    }
}
