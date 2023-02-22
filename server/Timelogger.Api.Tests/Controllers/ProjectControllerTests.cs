using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Timelogger.Api.Controllers;
using Timelogger.Application.Interfaces.Repositories;
using Timelogger.Application.Interfaces.Services;
using Timelogger.Application.Services;
using Timelogger.Application.ViewModels.Responses;
using Timelogger.Domain.Entities;
using Timelogger.Infrastructure.Repositories;

namespace Timelogger.Api.Tests.Controllers
{
    public class ProjectControllerTests : BaseTest
    {
        private IProjectService _projectService;
        private ProjectController _projectController;
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
            _projectController = new ProjectController(_projectService);

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
        public void GetAll_ValidRequest_ReturnProjectsList()
        {
            // Arrange
            var projects = new List<Project>() {
                new Project { ProjectName = "Test Project 1", CustomerId = 1, ProjectStageId = 1, Deadline = DateTime.Parse("2023/02/23") },
                new Project { ProjectName = "Test Project 2", CustomerId = 1, ProjectStageId = 1, Deadline = DateTime.Parse("2023/02/22") },
                new Project { ProjectName = "Test Project 3", CustomerId = 1, ProjectStageId = 1, Deadline = DateTime.Parse("2023/02/21") } };
            AddProject(projects);

            var pageIndex = 1;
            var pageSize = 1;
            string sortBy = "deadline";
            string sortDirection = "asc";

            // Act
            var result = _projectController.GetAll(sortBy, sortDirection, pageIndex, pageSize).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<PaginatedList<ProjectResponse>>(result.Value);
            var paginatedList = result.Value as PaginatedList<ProjectResponse>;
            Assert.AreEqual(1, paginatedList.Items.Count);
            Assert.AreEqual(1, paginatedList.PageIndex);
            Assert.AreEqual(3, paginatedList.TotalPages);

            var projectResult = paginatedList.Items.FirstOrDefault();
            Assert.AreEqual(DateTime.Parse("2023/02/21"), projectResult.Deadline);
            Assert.AreEqual("Test Project 3", projectResult.ProjectName);
            Assert.AreEqual("Awaiting", projectResult.ProjectStageName);
            Assert.AreEqual("Visma", projectResult.CustomerName);
        }

        //TODO: The rest of the controller tests goes here. I've done only the first one to show the idea

        private void AddProject(List<Project> project)
        {
            _context.Projects.AddRange(project);
            _context.SaveChanges();
        }
    }
}
