using BizCompany.API.Entities;
using System.Security.Principal;

namespace BizCompany.API.DataAccess
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> RemoveAsync(int id);
    }
}
