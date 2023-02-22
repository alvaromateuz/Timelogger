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
    public class TimeLogControllerTests : BaseTest
    {
        private ITimeLogService _timeLogService;
        private TimeLogController _timeLogController;
        private IProjectRepository _projectRepository;
        private ITimeLogRepository _timeLogRepository;
        private IDeveloperRepository _developerRepository;

        [SetUp]
        public void Setup()
        {
            _projectRepository = new ProjectRepository(_context);
            _timeLogRepository = new TimeLogRepository(_context);
            _developerRepository = new DeveloperRepository(_context);

            _timeLogService = new TimeLogService(_timeLogRepository, _projectRepository, _developerRepository, _mapper);
            _timeLogController = new TimeLogController(_timeLogService);

            //Customers
            _context.Customers.AddRange(new[] {
                new Customer { CustomerName = "Visma" },
                new Customer { CustomerName = "Farfetch" } });

            //Developers
            _context.Developers.AddRange(new[] {
                new Developer { DeveloperName = "Developer test 1" },
                new Developer { DeveloperName = "Developer test 2" } });

            //Projects
            _context.Projects.AddRange(new[] {
                new Project { ProjectId = 1, ProjectName = "Project test 1", CustomerId = 1, ProjectStageId = 1 },
                new Project { ProjectId = 2, ProjectName = "Project test 2", CustomerId = 1, ProjectStageId = 3 } });

            //ProjectStages
            _context.ProjectStages.AddRange(new[] {
                new ProjectStage { ProjectStageId = 1, ProjectStageName = "Awaiting" },
                new ProjectStage { ProjectStageId = 2, ProjectStageName = "Started" },
                new ProjectStage { ProjectStageId = 3, ProjectStageName = "Closed" } });

            _context.SaveChanges();
        }

        [Test]
        public void GetAll_ValidRequest_ReturnTimeLogsList()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog { DeveloperId = 1, ProjectId = 1, LogDate = DateTime.Parse("2023/02/23"), TimeSpent = 30 },
                new TimeLog { DeveloperId = 1, ProjectId = 1, LogDate = DateTime.Parse("2023/02/22"), TimeSpent = 60 },
                new TimeLog { DeveloperId = 1, ProjectId = 1, LogDate = DateTime.Parse("2023/02/21"), TimeSpent = 90 } };
            AddTimeLog(timeLogs);

            var pageIndex = 1;
            var pageSize = 1;

            // Act
            var result = _timeLogController.GetAll(pageIndex, pageSize).Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf<PaginatedList<TimeLogResponse>>(result.Value);
            var paginatedList = result.Value as PaginatedList<TimeLogResponse>;
            Assert.AreEqual(1, paginatedList.Items.Count);
            Assert.AreEqual(1, paginatedList.PageIndex);
            Assert.AreEqual(3, paginatedList.TotalPages);

            var timeLogResult = paginatedList.Items.FirstOrDefault();
            Assert.AreEqual(DateTime.Parse("2023/02/21"), timeLogResult.LogDate);
            Assert.AreEqual(90, timeLogResult.TimeSpent);
            Assert.AreEqual("Visma", timeLogResult.CustomerName);
            Assert.AreEqual("Project test 1", timeLogResult.ProjectName);
            Assert.AreEqual("Developer test 1", timeLogResult.DeveloperName);
        }

        //TODO: The rest of the controller tests goes here. I've done only the first one to show the idea

        private void AddTimeLog(List<TimeLog> timeLog)
        {
            _context.TimeLogs.AddRange(timeLog);
            _context.SaveChanges();
        }
    }
}
