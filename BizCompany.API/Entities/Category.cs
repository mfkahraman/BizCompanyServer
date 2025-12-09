namespace BizCompany.API.Entities
{
    public class Category : IEntity
    {
        public int Id { get; set; }
        public required string CategoryName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IList<Product>? Products { get; set; }
    }
}
