using BizCompany.API.Context;
using Microsoft.EntityFrameworkCore;

namespace BizCompany.API.DataAccess
{
    public class BaseRepository<T>(AppDbContext context) 
        : IBaseRepository<T> where T : class
    {
        public async Task<bool> CreateAsync(T entity)
        {
            var result = await context.Set<T>().AddAsync(entity);
            if (result != null)
            {
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Update
    }
}
