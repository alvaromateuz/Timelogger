using System.Collections.Generic;
using System.Threading.Tasks;

namespace Timelogger.Application.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<TEntity> DeleteAsync(int id);
    }
}
