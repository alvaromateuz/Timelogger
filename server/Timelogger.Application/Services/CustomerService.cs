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
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerResponse> AddAsync(CustomerRequest request)
        {
            ValidateRequest(request);

            var customerEntity = _mapper.Map<Customer>(request);

            var result = await _customerRepository.AddAsync(customerEntity);

            return _mapper.Map<CustomerResponse>(result);
        }

        public async Task<CustomerResponse> DeleteAsync(int id)
        {
            var result = await _customerRepository.DeleteAsync(id);

            if (result == null)
                throw new TimelogException("Invalid CustomerId");

            return _mapper.Map<CustomerResponse>(result);
        }

        public async Task<PaginatedList<CustomerResponse>> GetAllAsync(int pageIndex, int pageSize)
        {
            if (pageIndex < 0 || pageSize < 0)
                throw new TimelogException("Page values should not be negative");

            var customers = await _customerRepository.GetAllAsync();

            var count = customers.Count;
            var items = customers
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var customerResponse = _mapper.Map<IEnumerable<Customer>, List<CustomerResponse>>(items);

            return new PaginatedList<CustomerResponse>(customerResponse, count, pageIndex, pageSize);
        }

        public async Task<CustomerResponse> GetByIdAsync(int id)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(id);

            return _mapper.Map<CustomerResponse>(existingCustomer);
        }

        public async Task<CustomerResponse> UpdateAsync(int id, CustomerRequest request)
        {
            ValidateRequest(request);

            var existingCustomer = await _customerRepository.GetByIdAsync(id);
            if (existingCustomer == null)
            {
                throw new TimelogException("Invalid CustomerId");
            }

            existingCustomer.CustomerName = request.CustomerName;

            var result = await _customerRepository.UpdateAsync(existingCustomer);
            
            if (result == null)
            {
                throw new TimelogException("Invalid CustomerId");
            }

            return _mapper.Map<CustomerResponse>(result);

        }

        private void ValidateRequest(CustomerRequest request)
        {
            if (request == null)
                throw new TimelogException("Invalid request");

            if (string.IsNullOrWhiteSpace(request.CustomerName) || request.CustomerName.Length > 30)
                throw new TimelogException("The customer name is not valid");
        }
    }
}
