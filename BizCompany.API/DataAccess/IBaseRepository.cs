using BizCompany.API.Entities;
using System.Security.Principal;

namespace BizCompany.API.DataAccess
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<IEntity>> GetAllAsync();
        Task<IEntity> GetByIdAsync(int id);
        Task<bool> CreateAsync(IEntity entity);
        Task<bool> UpdateAsync(IEntity entity);
        Task<bool> RemoveAsync(int id);
    }
}
