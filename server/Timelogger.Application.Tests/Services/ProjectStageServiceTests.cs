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
    public class ProjectStageServiceTests : BaseTest
    {
        private IProjectStageService _projectStageService;
        private IProjectStageRepository _projectStageRepository;

        [SetUp]
        public void Setup()
        {
            _projectStageRepository = new ProjectStageRepository(_context);
            _projectStageService = new ProjectStageService(_projectStageRepository, _mapper);
        }

        [Test]
        public void AddAsync_ValidRequest_ReturnProjectStageResponseWithId()
        {
            // Arrange
            var request = new ProjectStageRequest
            {
                ProjectStageName = "Test ProjectStage"
            };

            // Act
            var result = _projectStageService.AddAsync(request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(request.ProjectStageName, result.ProjectStageName);
            Assert.Greater(result.ProjectStageId, 0);
        }

        [Test]
        public void AddAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            var request = new ProjectStageRequest
            {
                ProjectStageName = ""
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectStageService.AddAsync(request));

            //Assert
            Assert.AreEqual("The projectStage name is not valid", ex.Message);
        }

        [Test]
        public void UpdateAsync_ValidRequest_ReturnProjectStageResponseWithId()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage" } };
            AddProjectStage(projectStages);

            var id = 1;
            var request = new ProjectStageRequest
            {
                ProjectStageName = "Test ProjectStage"
            };

            // Act
            var result = _projectStageService.UpdateAsync(id, request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(request.ProjectStageName, result.ProjectStageName);
            Assert.Greater(result.ProjectStageId, 0);
        }

        [Test]
        public void UpdateAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage" } };
            AddProjectStage(projectStages);

            var id = 1;
            var request = new ProjectStageRequest
            {
                ProjectStageName = ""
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectStageService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("The projectStage name is not valid", ex.Message);
        }

        [Test]
        public void UpdateAsync_InvalidId_ReturnException()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage" } };
            AddProjectStage(projectStages);

            var id = 123;
            var request = new ProjectStageRequest
            {
                ProjectStageName = "Test ProjectStage"
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectStageService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("Invalid ProjectStageId", ex.Message);
        }

        [Test]
        public void DeleteAsync_ValidRequest_ReturnProjectStageResponseWithId()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage" } };
            AddProjectStage(projectStages);

            var id = 1;

            // Act
            var result = _projectStageService.DeleteAsync(id).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(projectStages[0].ProjectStageName, result.ProjectStageName);
            Assert.AreEqual(projectStages[0].ProjectStageId, result.ProjectStageId);
        }

        [Test]
        public void DeleteAsync_InvalidId_ReturnException()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage" } };
            AddProjectStage(projectStages);

            var id = 123;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectStageService.DeleteAsync(id));

            // Assert
            Assert.AreEqual("Invalid ProjectStageId", ex.Message);
        }

        [Test]
        public void GetAllAsync_ValidRequest_ReturnProjectStagesList()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage 1" },
                new ProjectStage { ProjectStageName = "Test ProjectStage 2" },
                new ProjectStage { ProjectStageName = "Test ProjectStage 3" } };
            AddProjectStage(projectStages);

            const int pageIndex = 2;
            const int pageSize = 1;

            // Act
            var result = _projectStageService.GetAllAsync(pageIndex, pageSize).Result;

            // Assert
            Assert.AreEqual(pageIndex, result.PageIndex);
            Assert.AreEqual(3, result.TotalPages);
            Assert.AreEqual(1, result.Items.Count());

            var projectStage = result.Items.FirstOrDefault();
            Assert.NotNull(projectStage);
            Assert.AreEqual("Test ProjectStage 2", projectStage.ProjectStageName);
        }

        [Test]
        public void GetAllAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            const int pageIndex = 2;
            const int pageSize = -1;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectStageService.GetAllAsync(pageIndex, pageSize));

            // Assert
            Assert.AreEqual("Page values should not be negative", ex.Message);
        }

        [Test]
        public void GetByIdAsync_ValidRequest_ReturnProjectStage()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage 1" },
                new ProjectStage { ProjectStageName = "Test ProjectStage 2" },
                new ProjectStage { ProjectStageName = "Test ProjectStage 3" } };
            AddProjectStage(projectStages);

            const int id = 2;

            // Act
            var projectStage = _projectStageService.GetByIdAsync(id).Result;

            // Assert
            Assert.NotNull(projectStage);
            Assert.AreEqual("Test ProjectStage 2", projectStage.ProjectStageName);
            Assert.AreEqual(id, projectStage.ProjectStageId);
        }

        [Test]
        public void GetByIdAsync_InvalidId_ReturnNull()
        {
            // Arrange
            var projectStages = new List<ProjectStage>() {
                new ProjectStage { ProjectStageName = "Test ProjectStage 1" },
                new ProjectStage { ProjectStageName = "Test ProjectStage 2" },
                new ProjectStage { ProjectStageName = "Test ProjectStage 3" } };
            AddProjectStage(projectStages);

            // Arrange
            const int id = 123;

            // Act
            var projectStage = _projectStageService.GetByIdAsync(id).Result;

            // Assert
            Assert.Null(projectStage);
        }

        private void AddProjectStage(List<ProjectStage> projectStage)
        {
            _context.ProjectStages.AddRange(projectStage);
            _context.SaveChanges();
        }
    }
}