using System.Collections.Generic;
using System.Threading.Tasks;
using Timelogger.Application.ViewModels.Requests;
using Timelogger.Application.ViewModels.Responses;

namespace Timelogger.Application.Interfaces.Services
{
    public interface ITimeLogService : IBaseService<TimeLogRequest, TimeLogResponse>
    {
        Task<PaginatedList<TimeLogResponse>> SearchAsync(TimeLogSearchRequest timeLog, int pageIndex, int pageSize);
    }
}
