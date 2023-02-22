using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using Timelogger.Api.Controllers;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Application.Interfaces.Services;
using Timelogger.Application.Services;
using Timelogger.Application.ViewModels.Responses;
using Timelogger.Domain.Entities;
using Timelogger.Infrastructure.Repositories;

namespace Timelogger.Api.Tests.Controllers
{
    public class CustomerControllerTests : BaseTest
    {
        private ICustomerService _customerService;
        private CustomerController _customerController;
        private ICustomerRepository _customerRepository;

        [SetUp]
        public void Setup()
        {
            _customerRepository = new CustomerRepository(_context);
            _customerService = new CustomerService(_customerRepository, _mapper);
            _customerController = new CustomerController(_customerService);
        }

        [Test]
        public void GetAll_ValidRequest_ReturnCustomersList()
        {
            // Arrange
            var customers = new List<Customer>() {
                new Customer { CustomerName = "Test Customer 1" },
                new Customer { CustomerName = "Test Customer 2" },
                new Customer { CustomerName = "Test Customer 3" } };
            AddCustomer(customers);

            var pageIndex = 1;
            var pageSize = 1;

            // Act
            var result = _customerController.GetAll(pageIndex, pageSize).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<PaginatedList<CustomerResponse>>(result.Value);
            var paginatedList = result.Value as PaginatedList<CustomerResponse>;
            Assert.AreEqual(1, paginatedList.Items.Count);
            Assert.AreEqual(1, paginatedList.PageIndex);
            Assert.AreEqual(3, paginatedList.TotalPages);
        }

        //TODO: The rest of the controller tests goes here. I've done only the first one to show the idea

        private void AddCustomer(List<Customer> customer)
        {
            _context.Customers.AddRange(customer);
            _context.SaveChanges();
        }
    }
}
