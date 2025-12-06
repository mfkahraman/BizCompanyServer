namespace BizCompany.API.DTOs
{
    public class ProductDto
    {
        //Changed Productname to nullable in order to show fluent validation messages properly
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public int CategoryId { get; set; }
    }
}
