using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timelogger.Application.Exceptions;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Application.Interfaces.Services;
using Timelogger.Application.ViewModels.Requests;
using Timelogger.Application.ViewModels.Responses;
using Timelogger.Domain.Entities;

namespace Timelogger.Application.Services
{
    public class TimeLogService : ITimeLogService
    {
        private const int MINIMUM_TIME_SPENT = 30;
        private const int PROJECT_CLOSED_STAGE = 3;

        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDeveloperRepository _developerRepository;
        private readonly IMapper _mapper;

        public TimeLogService(ITimeLogRepository timeLogRepository, IProjectRepository projectRepository, IDeveloperRepository developerRepository, IMapper mapper)
        {
            _timeLogRepository = timeLogRepository;
            _projectRepository = projectRepository;
            _developerRepository = developerRepository;
            _mapper = mapper;
        }

        public async Task<TimeLogResponse> AddAsync(TimeLogRequest request)
        {
            await ValidateRequestAsync(request);

            var timeLogEntity = _mapper.Map<TimeLog>(request);

            var result = await _timeLogRepository.AddAsync(timeLogEntity);

            return _mapper.Map<TimeLogResponse>(result);
        }

        public async Task<TimeLogResponse> DeleteAsync(int id)
        {
            var existingTimeLog = await _timeLogRepository.GetByIdAsync(id);
            if (existingTimeLog == null)
                throw new TimelogException("Invalid TimeLogId");

            var result = await _timeLogRepository.DeleteAsync(id);

            return _mapper.Map<TimeLogResponse>(result);
        }

        public async Task<PaginatedList<TimeLogResponse>> GetAllAsync(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
                throw new TimelogException("Page values should not be negative");

            var timeLogs = await _timeLogRepository.GetAllAsync();

            var count = timeLogs.Count;
            var items = timeLogs
                .OrderByDescending(c => c.TimeLogId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var timeLogResponse = _mapper.Map<IEnumerable<TimeLog>, List<TimeLogResponse>>(items);

            return new PaginatedList<TimeLogResponse>(timeLogResponse, count, pageIndex, pageSize);
        }

        public async Task<TimeLogResponse> GetByIdAsync(int id)
        {
            var existingTimeLog = await _timeLogRepository.GetByIdAsync(id);

            return _mapper.Map<TimeLogResponse>(existingTimeLog);
        }

        public async Task<PaginatedList<TimeLogResponse>> SearchAsync(TimeLogSearchRequest searchRequest, int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
                throw new TimelogException("Page values should not be negative");

            var timeLogs = await _timeLogRepository.GetAllAsync();

            if (searchRequest.ProjectId.HasValue)
                timeLogs = timeLogs.Where(t => t.ProjectId == searchRequest.ProjectId).ToList();

            if (searchRequest.DeveloperId.HasValue)
                timeLogs = timeLogs.Where(t => t.DeveloperId == searchRequest.DeveloperId).ToList();

            if (searchRequest.InitialDate.HasValue)
                timeLogs = timeLogs.Where(t => t.LogDate >= searchRequest.InitialDate).ToList();

            if (searchRequest.FinalDate.HasValue)
                timeLogs = timeLogs.Where(t => t.LogDate <= searchRequest.FinalDate).ToList();

            var count = timeLogs.Count;
            var items = timeLogs
                .OrderByDescending(c => c.TimeLogId)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var timeLogResponse = _mapper.Map<IEnumerable<TimeLog>, List<TimeLogResponse>>(items);

            return new PaginatedList<TimeLogResponse>(timeLogResponse, count, pageIndex, pageSize);

        }

        public async Task<TimeLogResponse> UpdateAsync(int id, TimeLogRequest request)
        {
            await ValidateRequestAsync(request);

            var existingTimeLog = await _timeLogRepository.GetByIdAsync(id);
            if (existingTimeLog == null)
            {
                throw new TimelogException("Invalid TimeLogId");
            }

            existingTimeLog.LogDate = request.LogDate;
            existingTimeLog.TimeSpent = request.TimeSpent;
            existingTimeLog.Description = request.Description;
            existingTimeLog.DeveloperId = request.DeveloperId;
            existingTimeLog.ProjectId = request.ProjectId;

            var result = await _timeLogRepository.UpdateAsync(existingTimeLog);

            if (result == null)
            {
                throw new TimelogException("Invalid TimeLogId");
            }

            return _mapper.Map<TimeLogResponse>(result);
        }

        private async Task ValidateRequestAsync(TimeLogRequest request)
        {

            if (request.TimeSpent < MINIMUM_TIME_SPENT)
                throw new TimelogException("Time spent must be more than 30 min");

            if(!string.IsNullOrEmpty(request.Description) && request.Description.Length>120)
                throw new TimelogException("The max size of description is 120 characters");

            var existingProject = await _projectRepository.GetByIdAsync(request.ProjectId);
            if (existingProject == null)
                throw new TimelogException("The project is not valid");

            if (existingProject.ProjectStageId == PROJECT_CLOSED_STAGE)
                throw new TimelogException("This project is already closed");

            var existingDeveloper = await _developerRepository.GetByIdAsync(request.DeveloperId);
            if(existingDeveloper == null)
                throw new TimelogException("The developer is not valid");
        }
    }
}
