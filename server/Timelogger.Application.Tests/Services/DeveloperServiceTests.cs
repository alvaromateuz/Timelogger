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
    public class DeveloperServiceTests : BaseTest
    {
        private IDeveloperService _developerService;
        private IDeveloperRepository _developerRepository;

        [SetUp]
        public void Setup()
        {
            _developerRepository = new DeveloperRepository(_context);
            _developerService = new DeveloperService(_developerRepository, _mapper);
        }

        [Test]
        public void AddAsync_ValidRequest_ReturnDeveloperResponseWithId()
        {
            // Arrange
            var request = new DeveloperRequest
            {
                DeveloperName = "Test Developer"
            };

            // Act
            var result = _developerService.AddAsync(request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(request.DeveloperName, result.DeveloperName);
            Assert.Greater(result.DeveloperId, 0);
        }

        [Test]
        public void AddAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            var request = new DeveloperRequest
            {
                DeveloperName = ""
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _developerService.AddAsync(request));

            //Assert
            Assert.AreEqual("The developer name is not valid", ex.Message);
        }

        [Test]
        public void UpdateAsync_ValidRequest_ReturnDeveloperResponseWithId()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer" } };
            AddDeveloper(developers);

            var id = 1;
            var request = new DeveloperRequest
            {
                DeveloperName = "Test Developer"
            };

            // Act
            var result = _developerService.UpdateAsync(id, request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(request.DeveloperName, result.DeveloperName);
            Assert.Greater(result.DeveloperId, 0);
        }

        [Test]
        public void UpdateAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer" } };
            AddDeveloper(developers);

            var id = 1;
            var request = new DeveloperRequest
            {
                DeveloperName = ""
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _developerService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("The developer name is not valid", ex.Message);
        }

        [Test]
        public void UpdateAsync_InvalidId_ReturnException()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer" } };
            AddDeveloper(developers);

            var id = 123;
            var request = new DeveloperRequest
            {
                DeveloperName = "Test Developer"
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _developerService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("Invalid DeveloperId", ex.Message);
        }

        [Test]
        public void DeleteAsync_ValidRequest_ReturnDeveloperResponseWithId()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer" } };
            AddDeveloper(developers);

            var id = 1;

            // Act
            var result = _developerService.DeleteAsync(id).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(developers[0].DeveloperName, result.DeveloperName);
            Assert.AreEqual(developers[0].DeveloperId, result.DeveloperId);
        }

        [Test]
        public void DeleteAsync_InvalidId_ReturnException()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer" } };
            AddDeveloper(developers);

            var id = 123;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _developerService.DeleteAsync(id));

            // Assert
            Assert.AreEqual("Invalid DeveloperId", ex.Message);
        }

        [Test]
        public void GetAllAsync_ValidRequest_ReturnDevelopersList()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer 1" },
                new Developer { DeveloperName = "Test Developer 2" },
                new Developer { DeveloperName = "Test Developer 3" } };
            AddDeveloper(developers);

            const int pageIndex = 2;
            const int pageSize = 1;

            // Act
            var result = _developerService.GetAllAsync(pageIndex, pageSize).Result;

            // Assert
            Assert.AreEqual(pageIndex, result.PageIndex);
            Assert.AreEqual(3, result.TotalPages);
            Assert.AreEqual(1, result.Items.Count());

            var developer = result.Items.FirstOrDefault();
            Assert.NotNull(developer);
            Assert.AreEqual("Test Developer 2", developer.DeveloperName);
        }

        [Test]
        public void GetAllAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            const int pageIndex = 2;
            const int pageSize = -1;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _developerService.GetAllAsync(pageIndex, pageSize));

            // Assert
            Assert.AreEqual("Page values should not be negative", ex.Message);
        }

        [Test]
        public void GetByIdAsync_ValidRequest_ReturnDeveloper()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer 1" },
                new Developer { DeveloperName = "Test Developer 2" },
                new Developer { DeveloperName = "Test Developer 3" } };
            AddDeveloper(developers);

            const int id = 2;

            // Act
            var developer = _developerService.GetByIdAsync(id).Result;

            // Assert
            Assert.NotNull(developer);
            Assert.AreEqual("Test Developer 2", developer.DeveloperName);
            Assert.AreEqual(id, developer.DeveloperId);
        }

        [Test]
        public void GetByIdAsync_InvalidId_ReturnNull()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer 1" },
                new Developer { DeveloperName = "Test Developer 2" },
                new Developer { DeveloperName = "Test Developer 3" } };
            AddDeveloper(developers);

            // Arrange
            const int id = 123;

            // Act
            var developer = _developerService.GetByIdAsync(id).Result;

            // Assert
            Assert.Null(developer);
        }

        private void AddDeveloper(List<Developer> developer)
        {
            _context.Developers.AddRange(developer);
            _context.SaveChanges();
        }
    }
}