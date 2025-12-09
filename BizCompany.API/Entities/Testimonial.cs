namespace BizCompany.API.Entities
{
    public class Testimonial : IEntity
    {
        public int Id { get; set; }
        public string? ClientName { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public string? ImageUrl{ get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
