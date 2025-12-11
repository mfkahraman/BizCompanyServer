using BizCompany.API.Context;
using BizCompany.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace BizCompany.API.DataAccess
{
    public class BaseRepository<T>(AppDbContext context)
        : IBaseRepository<T> where T : class, IEntity
    {
        public async Task<bool> CreateAsync(T entity)
        {
            var result = await context.Set<T>().AddAsync(entity);
            if (result != null)
            {
                var saveResult = await context.SaveChangesAsync();
                if(saveResult > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>()
                                .Where(x => !x.IsDeleted)
                                .AsNoTracking()
                                .ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>()
                          .Where(x => !x.IsDeleted && x.Id == id)
                          .FirstOrDefaultAsync()
                          ?? throw new KeyNotFoundException($"{typeof(T).Name} with id {id} not found.");
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var entity = await context.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted)
                ?? throw new KeyNotFoundException($"{typeof(T).Name} with id {id} not found.");

            entity.IsDeleted = true;
            context.Set<T>().Update(entity);
            return await context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            return await context.SaveChangesAsync() > 0;
        }
    }
}
