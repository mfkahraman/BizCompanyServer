using BizCompany.API.Context;
using BizCompany.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BizCompany.API.DataAccess
{
    public class ProductRepository (AppDbContext context)
    {
        public async Task<List<Product>> GetProductsWithDetailAsync()
        {
            var products = await context.Products
                .Where(p => !p.IsDeleted && p.Category != null && !p.Category.IsDeleted)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
            return products;
        }
    }
}
