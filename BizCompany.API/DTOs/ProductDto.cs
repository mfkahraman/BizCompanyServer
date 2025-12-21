namespace BizCompany.API.DTOs
{
    public class ProductDto
    {
        //Changed Productname to nullable in order to show fluent validation messages properly
        public string? ProductName { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public string? ClientName { get; set; }
        public string? ProjectUrl { get; set; }
        public DateTime? ProjectDate { get; set; }

        public IFormFile? HeroImage { get; set; }
        public IFormFile? ThumbnailImage { get; set; }
    }
}
