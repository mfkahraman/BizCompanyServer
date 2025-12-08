namespace BizCompany.API.Entities
{
    public class BlogParagraph
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int Order { get; set; }
        public bool IsDeleted { get; set; }
    }
}
