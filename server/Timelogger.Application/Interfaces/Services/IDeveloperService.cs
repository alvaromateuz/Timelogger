using System.Threading.Tasks;
using Timelogger.Application.ViewModels.Requests;
using Timelogger.Application.ViewModels.Responses;

namespace Timelogger.Application.Interfaces.Services
{
    public interface IDeveloperService : IBaseService<DeveloperRequest, DeveloperResponse>
    {
    }
}
