using BizCompany.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BizCompany.API.Context
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
