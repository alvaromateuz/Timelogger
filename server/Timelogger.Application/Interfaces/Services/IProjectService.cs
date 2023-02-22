using System.Threading.Tasks;
using Timelogger.Application.ViewModels.Requests;
using Timelogger.Application.ViewModels.Responses;

namespace Timelogger.Application.Interfaces.Services
{
    public interface IProjectService : IBaseService<ProjectRequest, ProjectResponse>
    {
        Task<PaginatedList<ProjectResponse>> GetAllAsync(int pageIndex, int pageSize, string sortBy, string sortDirection);
    }
}
