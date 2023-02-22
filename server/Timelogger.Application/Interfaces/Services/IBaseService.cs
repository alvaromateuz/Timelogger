using System.Collections.Generic;
using System.Threading.Tasks;
using Timelogger.Application.ViewModels.Responses;

namespace Timelogger.Application.Interfaces.Services
{
    public interface IBaseService<TRequest, TResponse>
    {
        Task<PaginatedList<TResponse>> GetAllAsync(int pageIndex, int pageSize);
        Task<TResponse> GetByIdAsync(int id);
        Task<TResponse> AddAsync(TRequest request);
        Task<TResponse> UpdateAsync(int id, TRequest request);
        Task<TResponse> DeleteAsync(int id);
    }
}
