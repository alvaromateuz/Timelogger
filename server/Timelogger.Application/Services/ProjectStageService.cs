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
    public class ProjectStageService : IProjectStageService
    {
        private readonly IMapper _mapper;
        private readonly IProjectStageRepository _projectStageRepository;

        public ProjectStageService(IProjectStageRepository projectStageRepository, IMapper mapper)
        {
            _projectStageRepository = projectStageRepository;
            _mapper = mapper;
        }

        public async Task<ProjectStageResponse> AddAsync(ProjectStageRequest request)
        {
            ValidateRequest(request);

            var projectStageEntity = _mapper.Map<ProjectStage>(request);

            var result = await _projectStageRepository.AddAsync(projectStageEntity);

            return _mapper.Map<ProjectStageResponse>(result);
        }

        public async Task<ProjectStageResponse> DeleteAsync(int id)
        {
            var existingProjectStage = await _projectStageRepository.GetByIdAsync(id);
            if (existingProjectStage == null)
                throw new TimelogException("Invalid ProjectStageId");

            var result = await _projectStageRepository.DeleteAsync(id);

            return _mapper.Map<ProjectStageResponse>(result);
        }

        public async Task<PaginatedList<ProjectStageResponse>> GetAllAsync(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
                throw new TimelogException("Page values should not be negative");

            var projectStages = await _projectStageRepository.GetAllAsync();

            var count = projectStages.Count;
            var items = projectStages
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var projectStageResponse = _mapper.Map<IEnumerable<ProjectStage>, List<ProjectStageResponse>>(items);

            return new PaginatedList<ProjectStageResponse>(projectStageResponse, count, pageIndex, pageSize);
        }

        public async Task<ProjectStageResponse> GetByIdAsync(int id)
        {
            var existingProjectStage = await _projectStageRepository.GetByIdAsync(id);

            return _mapper.Map<ProjectStageResponse>(existingProjectStage);
        }

        public async Task<ProjectStageResponse> UpdateAsync(int id, ProjectStageRequest request)
        {
            ValidateRequest(request);

            var existingProjectStage = await _projectStageRepository.GetByIdAsync(id);
            if (existingProjectStage == null)
            {
                throw new TimelogException("Invalid ProjectStageId");
            }

            existingProjectStage.ProjectStageName = request.ProjectStageName;

            var result = await _projectStageRepository.UpdateAsync(existingProjectStage);

            if (result == null)
            {
                throw new TimelogException("Invalid ProjectStageId");
            }

            return _mapper.Map<ProjectStageResponse>(result);
        }

        private void ValidateRequest(ProjectStageRequest request)
        {
            if (request == null)
                throw new TimelogException("Invalid request");

            if (string.IsNullOrWhiteSpace(request.ProjectStageName) || request.ProjectStageName.Length > 30)
                throw new TimelogException("The projectStage name is not valid");
        }
    }
}
