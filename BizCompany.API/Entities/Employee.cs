namespace BizCompany.API.Entities
{
    public class Employee
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
