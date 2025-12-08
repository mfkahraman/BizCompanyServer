namespace BizCompany.API.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int? BlogId { get; set; } //Delete Behavior için nullable yaptım
        public Blog? Blog { get; set; } 
        public int? WriterId { get; set; } //Delete Behavior için nullable yaptım
        public Writer? Writer { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
