namespace BizCompany.API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }
    }
}
