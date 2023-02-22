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
    public class DeveloperService : IDeveloperService
    {
        private readonly IMapper _mapper;
        private readonly IDeveloperRepository _developerRepository;

        public DeveloperService(IDeveloperRepository developerRepository, IMapper mapper)
        {
            _developerRepository = developerRepository;
            _mapper = mapper;
        }

        public async Task<DeveloperResponse> AddAsync(DeveloperRequest request)
        {
            ValidateRequest(request);

            var developerEntity = _mapper.Map<Developer>(request);

            var result = await _developerRepository.AddAsync(developerEntity);

            return _mapper.Map<DeveloperResponse>(result);
        }

        public async Task<DeveloperResponse> DeleteAsync(int id)
        {
            var existingDeveloper = await _developerRepository.GetByIdAsync(id);
            if (existingDeveloper == null)
                throw new TimelogException("Invalid DeveloperId");

            var result = await _developerRepository.DeleteAsync(id);

            return _mapper.Map<DeveloperResponse>(result);
        }

        public async Task<PaginatedList<DeveloperResponse>> GetAllAsync(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
                throw new TimelogException("Page values should not be negative");

            var developers = await _developerRepository.GetAllAsync();

            var count = developers.Count;
            var items = developers
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var developerResponse = _mapper.Map<IEnumerable<Developer>, List<DeveloperResponse>>(items);

            return new PaginatedList<DeveloperResponse>(developerResponse, count, pageIndex, pageSize);
        }

        public async Task<DeveloperResponse> GetByIdAsync(int id)
        {
            var existingDeveloper = await _developerRepository.GetByIdAsync(id);

            return _mapper.Map<DeveloperResponse>(existingDeveloper);
        }

        public async Task<DeveloperResponse> UpdateAsync(int id, DeveloperRequest request)
        {
            ValidateRequest(request);

            var existingDeveloper = await _developerRepository.GetByIdAsync(id);
            if (existingDeveloper == null)
            {
                throw new TimelogException("Invalid DeveloperId");
            }

            existingDeveloper.DeveloperName = request.DeveloperName;

            var result = await _developerRepository.UpdateAsync(existingDeveloper);

            if (result == null)
            {
                throw new TimelogException("Invalid DeveloperId");
            }

            return _mapper.Map<DeveloperResponse>(result);
        }

        private void ValidateRequest(DeveloperRequest request)
        {
            if (request == null)
                throw new TimelogException("Invalid request");

            if (string.IsNullOrWhiteSpace(request.DeveloperName) || request.DeveloperName.Length > 30)
                throw new TimelogException("The developer name is not valid");
        }
    }
}
