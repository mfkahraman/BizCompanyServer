namespace BizCompany.API.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string CategoryName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual IList<Product>? Products { get; set; }
    }
}
