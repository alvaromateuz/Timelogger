using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Timelogger.Application.Exceptions;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Application.Interfaces.Services;
using Timelogger.Application.Services;
using Timelogger.Application.ViewModels.Requests;
using Timelogger.Domain.Entities;
using Timelogger.Infrastructure.Repositories;

namespace Timelogger.Application.Tests.Services
{
    [TestFixture]
    public class CustomerServiceTests : BaseTest
    {
        private ICustomerService _customerService;
        private ICustomerRepository _customerRepository;

        [SetUp]
        public void Setup()
        {
            _customerRepository = new CustomerRepository(_context);
            _customerService = new CustomerService(_customerRepository, _mapper);
        }

        [Test]
        public void AddAsync_ValidRequest_ReturnCustomerResponseWithId()
        {
            // Arrange
            var request = new CustomerRequest
            {
                CustomerName = "Test Customer"
            };

            // Act
            var result = _customerService.AddAsync(request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(request.CustomerName, result.CustomerName);
            Assert.Greater(result.CustomerId, 0);
        }

        [Test]
        public void AddAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            var request = new CustomerRequest
            {
                CustomerName = ""
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _customerService.AddAsync(request));

            //Assert
            Assert.AreEqual("The customer name is not valid", ex.Message);
        }

        [Test]
        public void UpdateAsync_ValidRequest_ReturnCustomerResponseWithId()
        {
            // Arrange
            var customers = new List<Customer>() { 
                new Customer { CustomerName = "Test Customer" } };
            AddCustomer(customers);

            var id = 1;
            var request = new CustomerRequest
            {
                CustomerName = "Test Customer"
            };

            // Act
            var result = _customerService.UpdateAsync(id, request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(request.CustomerName, result.CustomerName);
            Assert.Greater(result.CustomerId, 0);
        }

        [Test]
        public void UpdateAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            var customers = new List<Customer>() {
                new Customer { CustomerName = "Test Customer" } };
            AddCustomer(customers);

            var id = 1;
            var request = new CustomerRequest
            {
                CustomerName = ""
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _customerService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("The customer name is not valid", ex.Message);
        }

        [Test]
        public void UpdateAsync_InvalidId_ReturnException()
        {
            // Arrange
            var customers = new List<Customer>() {
                new Customer { CustomerName = "Test Customer" } };
            AddCustomer(customers);

            var id = 123;
            var request = new CustomerRequest
            {
                CustomerName = "Test Customer"
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _customerService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("Invalid CustomerId", ex.Message);
        }

        [Test]
        public void DeleteAsync_ValidRequest_ReturnCustomerResponseWithId()
        {
            // Arrange
            var customers = new List<Customer>() {
                new Customer { CustomerName = "Test Customer" } };
            AddCustomer(customers);

            var id = 1;

            // Act
            var result = _customerService.DeleteAsync(id).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(customers[0].CustomerName, result.CustomerName);
            Assert.AreEqual(customers[0].CustomerId, result.CustomerId);
        }

        [Test]
        public void DeleteAsync_InvalidId_ReturnException()
        {
            // Arrange
            var customers = new List<Customer>() {
                new Customer { CustomerName = "Test Customer" } };
            AddCustomer(customers);

            var id = 123;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _customerService.DeleteAsync(id));

            // Assert
            Assert.AreEqual("Invalid CustomerId", ex.Message);
        }

        [Test]
        public void GetAllAsync_ValidRequest_ReturnCustomersList()
        {
            // Arrange
            var customers = new List<Customer>() {
                new Customer { CustomerName = "Test Customer 1" },
                new Customer { CustomerName = "Test Customer 2" },
                new Customer { CustomerName = "Test Customer 3" } };
            AddCustomer(customers);

            const int pageIndex = 2;
            const int pageSize = 1;

            // Act
            var result = _customerService.GetAllAsync(pageIndex, pageSize).Result;

            // Assert
            Assert.AreEqual(pageIndex, result.PageIndex);
            Assert.AreEqual(3, result.TotalPages);
            Assert.AreEqual(1, result.Items.Count());

            var customer = result.Items.FirstOrDefault();
            Assert.NotNull(customer);
            Assert.AreEqual("Test Customer 2", customer.CustomerName);
        }

        [Test]
        public void GetAllAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            const int pageIndex = 2;
            const int pageSize = -1;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _customerService.GetAllAsync(pageIndex, pageSize));

            // Assert
            Assert.AreEqual("Page values should not be negative", ex.Message);
        }

        [Test]
        public void GetByIdAsync_ValidRequest_ReturnCustomer()
        {
            // Arrange
            var customers = new List<Customer>() {
                new Customer { CustomerName = "Test Customer 1" },
                new Customer { CustomerName = "Test Customer 2" },
                new Customer { CustomerName = "Test Customer 3" } };
            AddCustomer(customers);

            const int id = 2;

            // Act
            var customer = _customerService.GetByIdAsync(id).Result;

            // Assert
            Assert.NotNull(customer);
            Assert.AreEqual("Test Customer 2", customer.CustomerName);
            Assert.AreEqual(id, customer.CustomerId);
        }

        [Test]
        public void GetByIdAsync_InvalidId_ReturnNull()
        {
            // Arrange
            var customers = new List<Customer>() {
                new Customer { CustomerName = "Test Customer 1" },
                new Customer { CustomerName = "Test Customer 2" },
                new Customer { CustomerName = "Test Customer 3" } };
            AddCustomer(customers);

            // Arrange
            const int id = 123;

            // Act
            var customer = _customerService.GetByIdAsync(id).Result;

            // Assert
            Assert.Null(customer);
        }

        private void AddCustomer(List<Customer> customer)
        {
            _context.Customers.AddRange(customer);
            _context.SaveChanges();
        }
    }
}