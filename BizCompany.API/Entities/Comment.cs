namespace BizCompany.API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int WriterId { get; set; }
        public Writer? Writer { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
