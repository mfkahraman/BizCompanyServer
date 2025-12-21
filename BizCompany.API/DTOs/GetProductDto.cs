namespace BizCompany.API.DTOs
{
    public class GetProductDto
    {
        public int Id { get; set; }
        public required string ProductName { get; set; }
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public string? ImagePath { get; set; }
        public string? ThumbnailImagePath { get; set; }
        public int? CategoryId { get; set; }
        public string? ClientName { get; set; }
        public string? ProjectUrl { get; set; }
        public DateTime? ProjectDate { get; set; }
    }
}
