namespace BizCompany.API.DTOs
{
    public class ProductDto
    {
        public required string ProductName { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int CategoryId { get; set; }
    }
}
