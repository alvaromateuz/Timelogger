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
    public class DeveloperControllerTests : BaseTest
    {
        private IDeveloperService _developerService;
        private DeveloperController _developerController;
        private IDeveloperRepository _developerRepository;

        [SetUp]
        public void Setup()
        {
            _developerRepository = new DeveloperRepository(_context);
            _developerService = new DeveloperService(_developerRepository, _mapper);
            _developerController = new DeveloperController(_developerService);
        }

        [Test]
        public void GetAll_ValidRequest_ReturnDevelopersList()
        {
            // Arrange
            var developers = new List<Developer>() {
                new Developer { DeveloperName = "Test Developer 1" },
                new Developer { DeveloperName = "Test Developer 2" },
                new Developer { DeveloperName = "Test Developer 3" } };
            AddDeveloper(developers);

            var pageIndex = 1;
            var pageSize = 1;

            // Act
            var result = _developerController.GetAll(pageIndex, pageSize).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<PaginatedList<DeveloperResponse>>(result.Value);
            var paginatedList = result.Value as PaginatedList<DeveloperResponse>;
            Assert.AreEqual(1, paginatedList.Items.Count);
            Assert.AreEqual(1, paginatedList.PageIndex);
            Assert.AreEqual(3, paginatedList.TotalPages);
        }

        //TODO: The rest of the controller tests goes here. I've done only the first one to show the idea

        private void AddDeveloper(List<Developer> developer)
        {
            _context.Developers.AddRange(developer);
            _context.SaveChanges();
        }
    }
}
