using NUnit.Framework;
using System;
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
    public class ProjectServiceTests : BaseTest
    {
        private IProjectService _projectService;
        private IProjectRepository _projectRepository;
        private ICustomerRepository _customerRepository;
        private IProjectStageRepository _projectStageRepository;

        [SetUp]
        public void Setup()
        {
            _projectRepository = new ProjectRepository(_context);
            _customerRepository = new CustomerRepository(_context);
            _projectStageRepository = new ProjectStageRepository(_context);
            _projectService = new ProjectService(_projectRepository, _customerRepository, _projectStageRepository, _mapper);

            //Customers
            _context.Customers.AddRange(new[] {
                new Customer { CustomerName = "Visma" },
                new Customer { CustomerName = "Farfetch" } });

            //ProjectStages
            _context.ProjectStages.AddRange(new[] {
                new ProjectStage { ProjectStageId = 1, ProjectStageName = "Awaiting" },
                new ProjectStage { ProjectStageId = 2, ProjectStageName = "Started" },
                new ProjectStage { ProjectStageId = 3, ProjectStageName = "Closed" } });
            
            _context.SaveChanges();
        }

        [Test]
        public void AddAsync_ValidRequest_ReturnProjectResponseWithId()
        {
            // Arrange
            var request = new ProjectRequest
            {
                ProjectName = "Test Project",
                ProjectStageId = 1,
                CustomerId = 1,
                Deadline = DateTime.Parse("2023/02/23")
            };

            // Act
            var result = _projectService.AddAsync(request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(request.ProjectName, result.ProjectName);
            Assert.AreEqual("Awaiting", result.ProjectStageName);
            Assert.AreEqual("Visma", result.CustomerName);
            Assert.AreEqual(request.Deadline, result.Deadline);
        }

        [Test]
        public void AddAsync_InvalidProjectName_ReturnException()
        {
            // Arrange
            var request = new ProjectRequest
            {
                ProjectName = "",
                ProjectStageId = 1,
                CustomerId = 1,
                Deadline = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectService.AddAsync(request));

            //Assert
            Assert.AreEqual("The project name is not valid", ex.Message);
        }

        [Test]
        public void AddAsync_InvalidStageId_ReturnException()
        {
            // Arrange
            var request = new ProjectRequest
            {
                ProjectName = "Test Project",
                ProjectStageId = 123,
                CustomerId = 1,
                Deadline = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectService.AddAsync(request));

            //Assert
            Assert.AreEqual("The project stage is not valid", ex.Message);
        }

        [Test]
        public void AddAsync_InvalidCustomerId_ReturnException()
        {
            // Arrange
            var request = new ProjectRequest
            {
                ProjectName = "Test Project",
                ProjectStageId = 1,
                CustomerId = 123,
                Deadline = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectService.AddAsync(request));

            //Assert
            Assert.AreEqual("The customer is invalid", ex.Message);
        }

        [Test]
        public void UpdateAsync_ValidRequest_ReturnProjectResponseWithId()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project { 
                    ProjectName = "Test Project", 
                    ProjectStageId = 1, 
                    CustomerId = 1, 
                    Deadline = DateTime.Parse("2023/02/23") } };
            AddProject(projects);

            var id = 1;
            var request = new ProjectRequest
            {
                ProjectName = "Test Project",
                ProjectStageId = 3,
                CustomerId = 1,
                Deadline = DateTime.Parse("2023/02/23")
            };

            // Act
            var result = _projectService.UpdateAsync(id, request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(request.ProjectName, result.ProjectName);
            Assert.AreEqual("Closed", result.ProjectStageName);
            Assert.AreEqual("Visma", result.CustomerName);
            Assert.AreEqual(request.Deadline, result.Deadline);
            Assert.AreEqual(id, result.ProjectId);
        }

        [Test]
        public void UpdateAsync_InvalidProjectName_ReturnException()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project {
                    ProjectName = "Test Project",
                    ProjectStageId = 1,
                    CustomerId = 1,
                    Deadline = DateTime.Parse("2023/02/23") } };
            AddProject(projects);

            var id = 1;
            var request = new ProjectRequest
            {
                ProjectName = "",
                ProjectStageId = 3,
                CustomerId = 1,
                Deadline = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("The project name is not valid", ex.Message);
        }

        [Test]
        public void UpdateAsync_InvalidId_ReturnException()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project {
                    ProjectName = "Test Project",
                    ProjectStageId = 1,
                    CustomerId = 1,
                    Deadline = DateTime.Parse("2023/02/23") } };
            AddProject(projects);

            var id = 123;
            var request = new ProjectRequest
            {
                ProjectName = "Test Project",
                ProjectStageId = 3,
                CustomerId = 1,
                Deadline = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("Invalid ProjectId", ex.Message);
        }

        [Test]
        public void DeleteAsync_ValidRequest_ReturnProjectResponseWithId()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project {
                    ProjectName = "Test Project",
                    ProjectStageId = 1,
                    CustomerId = 1,
                    Deadline = DateTime.Parse("2023/02/23") } };
            AddProject(projects);

            var id = 1;

            // Act
            var result = _projectService.DeleteAsync(id).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(projects[0].ProjectName, result.ProjectName);
            Assert.AreEqual(projects[0].ProjectId, result.ProjectId);
        }

        [Test]
        public void DeleteAsync_InvalidId_ReturnException()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project {
                    ProjectName = "Test Project",
                    ProjectStageId = 1,
                    CustomerId = 1,
                    Deadline = DateTime.Parse("2023/02/23") } };
            AddProject(projects);

            var id = 123;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectService.DeleteAsync(id));

            // Assert
            Assert.AreEqual("Invalid ProjectId", ex.Message);
        }

        [Test]
        public void GetAllAsync_ValidRequest_ReturnProjectsList()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project { ProjectName = "Test Project 1", CustomerId = 1, ProjectStageId = 1 },
                new Project { ProjectName = "Test Project 2", CustomerId = 1, ProjectStageId = 1 },
                new Project { ProjectName = "Test Project 3", CustomerId = 1, ProjectStageId = 1 } };
            AddProject(projects);

            const int pageIndex = 2;
            const int pageSize = 1;

            // Act
            var result = _projectService.GetAllAsync(pageIndex, pageSize).Result;

            // Assert
            Assert.AreEqual(pageIndex, result.PageIndex);
            Assert.AreEqual(3, result.TotalPages);
            Assert.AreEqual(1, result.Items.Count());

            var project = result.Items.FirstOrDefault();
            Assert.NotNull(project);
            Assert.AreEqual("Test Project 2", project.ProjectName);
        }

        [Test]
        public void GetAllAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            const int pageIndex = 2;
            const int pageSize = -1;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _projectService.GetAllAsync(pageIndex, pageSize));

            // Assert
            Assert.AreEqual("Page values should not be negative", ex.Message);
        }

        [Test]
        public void GetByIdAsync_ValidRequest_ReturnProject()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project { ProjectName = "Test Project 1" },
                new Project { ProjectName = "Test Project 2" },
                new Project { ProjectName = "Test Project 3" } };
            AddProject(projects);

            const int id = 2;

            // Act
            var project = _projectService.GetByIdAsync(id).Result;

            // Assert
            Assert.NotNull(project);
            Assert.AreEqual("Test Project 2", project.ProjectName);
            Assert.AreEqual(id, project.ProjectId);
        }

        [Test]
        public void GetByIdAsync_InvalidId_ReturnNull()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project { ProjectName = "Test Project 1" },
                new Project { ProjectName = "Test Project 2" },
                new Project { ProjectName = "Test Project 3" } };
            AddProject(projects);

            // Arrange
            const int id = 123;

            // Act
            var project = _projectService.GetByIdAsync(id).Result;

            // Assert
            Assert.Null(project);
        }

        private void AddProject(List<Project> project)
        {
            _context.Projects.AddRange(project);
            _context.SaveChanges();
        }
    }
}