namespace BizCompany.API.DTOs
{
    public class GetTestimonialDto
    {
        public int Id { get; set; }
        public string? ClientName { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public string? ImageUrl { get; set; }
    }
}
