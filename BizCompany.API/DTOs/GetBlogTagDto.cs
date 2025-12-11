using BizCompany.API.Entities;

namespace BizCompany.API.DTOs
{
    public class GetBlogTagDto
    {
        public int Id { get; set; }
        public int BlogId { get; set; }
        public int TagId { get; set; }
    }
}
