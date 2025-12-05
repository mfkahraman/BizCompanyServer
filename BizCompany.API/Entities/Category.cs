namespace BizCompany.API.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public required string CategoryName { get; set; }
        public virtual IList<Product>? Products { get; set; }
    }
}
