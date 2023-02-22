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
    public class ProjectStageControllerTests : BaseTest
    {
        private IProjectStageService _projectStageService;
        private ProjectStageController _projectStageController;
        private IProjectStageRepository _projectStageRepository;

        [SetUp]
        public void Setup()
        {
            _projectStageRepository = new ProjectStageRepository(_context);
            _projectStageService = new ProjectStageService(_projectStageRepository, _mapper);
            _projectStageController = new ProjectStageController(_projectStageService);
        }

        [Test]
        public void GetAll_ValidRequest_ReturnProjectStagesList()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage 1" },
                new ProjectStage { ProjectStageName = "Test ProjectStage 2" },
                new ProjectStage { ProjectStageName = "Test ProjectStage 3" } };
            AddProjectStage(projectStages);

            var pageIndex = 1;
            var pageSize = 1;

            // Act
            var result = _projectStageController.GetAll(pageIndex, pageSize).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<PaginatedList<ProjectStageResponse>>(result.Value);
            var paginatedList = result.Value as PaginatedList<ProjectStageResponse>;
            Assert.AreEqual(1, paginatedList.Items.Count);
            Assert.AreEqual(1, paginatedList.PageIndex);
            Assert.AreEqual(3, paginatedList.TotalPages);
        }

        //TODO: The rest of the controller tests goes here. I've done only the first one to show the idea

        private void AddProjectStage(List<ProjectStage> projectStage)
        {
            _context.ProjectStages.AddRange(projectStage);
            _context.SaveChanges();
        }
    }
}
