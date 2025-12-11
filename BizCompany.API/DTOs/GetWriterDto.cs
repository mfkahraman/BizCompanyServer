namespace BizCompany.API.DTOs
{
    public class GetWriterDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
    }
}
