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
    public class ProjectService : IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;
        private ICustomerRepository _customerRepository;
        private IProjectStageRepository _projectStageRepository;

        public ProjectService(IProjectRepository projectRepository, ICustomerRepository customerRepository, IProjectStageRepository projectStageRepository, IMapper mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _customerRepository = customerRepository;
            _projectStageRepository = projectStageRepository;
        }

        public async Task<ProjectResponse> AddAsync(ProjectRequest request)
        {
            ValidateRequest(request);

            var projectEntity = _mapper.Map<Project>(request);
            
            var result = await _projectRepository.AddAsync(projectEntity);

            return _mapper.Map<ProjectResponse>(result);
        }

        public async Task<ProjectResponse> DeleteAsync(int id)
        {
            var existingProject = await _projectRepository.GetByIdAsync(id);
            if (existingProject == null)
                throw new TimelogException("Invalid ProjectId");

            var result = await _projectRepository.DeleteAsync(id);

            return _mapper.Map<ProjectResponse>(result);
        }

        public async Task<PaginatedList<ProjectResponse>> GetAllAsync(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
                throw new TimelogException("Page values should not be negative");

            var projects = await _projectRepository.GetAllAsync();

            var count = projects.Count;
            var items = projects
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var projectResponse = _mapper.Map<IEnumerable<Project>, List<ProjectResponse>>(items);

            return new PaginatedList<ProjectResponse>(projectResponse, count, pageIndex, pageSize);
        }

        public async Task<PaginatedList<ProjectResponse>> GetAllAsync(int pageIndex, int pageSize, string sortBy, string sortDirection)
        {
            var projects = await _projectRepository.GetAllAsync();

            projects = sortBy switch
            {
                "deadline" => sortDirection == "desc" ? projects.OrderByDescending(p => p.Deadline).ToList() : projects.OrderBy(p => p.Deadline).ToList(),
                "projectName" => sortDirection == "desc" ? projects.OrderByDescending(p => p.ProjectName).ToList() : projects.OrderBy(p => p.ProjectName).ToList(),
                _ => projects.OrderBy(p => p.Deadline).ToList(),
            };

            

            var count = projects.Count;
            var items = projects
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var projectsResponse = _mapper.Map<IEnumerable<Project>, List<ProjectResponse>>(items);

            return new PaginatedList<ProjectResponse>(projectsResponse, count, pageIndex, pageSize);
        }

        public async Task<ProjectResponse> GetByIdAsync(int id)
        {
            var existingProject = await _projectRepository.GetByIdAsync(id);

            return _mapper.Map<ProjectResponse>(existingProject);
        }

        public async Task<ProjectResponse> UpdateAsync(int id, ProjectRequest request)
        {
            ValidateRequest(request);

            var existingProject = await _projectRepository.GetByIdAsync(id);
            if (existingProject == null)
            {
                throw new TimelogException("Invalid ProjectId");
            }

            existingProject.ProjectName = request.ProjectName;
            existingProject.Deadline = request.Deadline;
            existingProject.ProjectStageId = request.ProjectStageId;
            existingProject.CustomerId = request.CustomerId;

            var result = await _projectRepository.UpdateAsync(existingProject);

            if (result == null)
            {
                throw new TimelogException("Invalid ProjectId");
            }

            return _mapper.Map<ProjectResponse>(result);
        }

        private void ValidateRequest(ProjectRequest request)
        {
            if (request == null)
                throw new TimelogException("Invalid request");

            if (string.IsNullOrWhiteSpace(request.ProjectName) || request.ProjectName.Length > 30)
                throw new TimelogException("The project name is not valid");

            var existingProjectStage = _projectStageRepository.GetByIdAsync(request.ProjectStageId).Result;
            if(existingProjectStage == null)
                throw new TimelogException("The project stage is not valid");

            var existingCustomer = _customerRepository.GetByIdAsync(request.CustomerId).Result;
            if( existingCustomer == null)
                throw new TimelogException("The customer is invalid");
        }
    }
}
