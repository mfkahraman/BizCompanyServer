namespace BizCompany.API.Entities
{
    public class Product : IEntity
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int? CategoryId { get; set; } //Delete Behavior için nullable yaptım
        public Category? Category { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
