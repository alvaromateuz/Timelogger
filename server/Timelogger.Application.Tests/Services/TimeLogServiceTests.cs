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
    public class TimeLogServiceTests : BaseTest
    {
        private ITimeLogService _timeLogService;
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

            _context.SaveChanges();
        }

        [Test]
        public void AddAsync_ValidRequest_ReturnTimeLogResponseWithId()
        {
            // Arrange
            var request = new TimeLogRequest
            {
                ProjectId = 1,
                DeveloperId = 1,
                TimeSpent = 30,
                LogDate = DateTime.Parse("2023/02/23")
            };

            // Act
            var result = _timeLogService.AddAsync(request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Project test 1", result.ProjectName);
            Assert.AreEqual("Developer test 1", result.DeveloperName);
            Assert.AreEqual(request.LogDate, result.LogDate);
            Assert.Greater(result.TimeLogId, 0);
        }

        [Test]
        public void AddAsync_InvalidProjectId_ReturnException()
        {
            // Arrange
            var request = new TimeLogRequest
            {
                ProjectId = 123,
                DeveloperId = 1,
                TimeSpent = 30,
                LogDate = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.AddAsync(request));

            //Assert
            Assert.AreEqual("The project is not valid", ex.Message);
        }

        [Test]
        public void AddAsync_InvaliDeveloperId_ReturnException()
        {
            // Arrange
            var request = new TimeLogRequest
            {
                ProjectId = 1,
                DeveloperId = 123,
                TimeSpent = 30,
                LogDate = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.AddAsync(request));

            //Assert
            Assert.AreEqual("The developer is not valid", ex.Message);
        }

        [Test]
        public void AddAsync_InvalidProjectStage_ReturnException()
        {
            // Arrange
            var request = new TimeLogRequest
            {
                ProjectId = 2, //Closed project
                DeveloperId = 1,
                TimeSpent = 30,
                LogDate = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.AddAsync(request));

            //Assert
            Assert.AreEqual("This project is already closed", ex.Message);
        }

        [Test]
        public void AddAsync_InvalidTimeSpent_ReturnException()
        {
            // Arrange
            var request = new TimeLogRequest
            {
                ProjectId = 1,
                DeveloperId = 1,
                TimeSpent = 28,
                LogDate = DateTime.Parse("2023/02/23")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.AddAsync(request));

            //Assert
            Assert.AreEqual("Time spent must be more than 30 min", ex.Message);
        }

        [Test]
        public void UpdateAsync_ValidRequest_ReturnTimeLogResponseWithId()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") } };
            AddTimeLog(timeLogs);

            var id = 1;
            var request = new TimeLogRequest
            {
                ProjectId = 1,
                DeveloperId = 2,
                TimeSpent = 120,
                LogDate = DateTime.Parse("2023/02/22")
            };

            // Act
            var result = _timeLogService.UpdateAsync(id, request).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Project test 1", result.ProjectName);
            Assert.AreEqual("Developer test 2", result.DeveloperName);
            Assert.AreEqual(request.TimeSpent, result.TimeSpent);
            Assert.AreEqual(request.LogDate, result.LogDate);
            Assert.AreEqual(id, result.TimeLogId);
        }

        [Test]
        public void UpdateAsync_InvalidTimeLogId_ReturnException()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") } };
            AddTimeLog(timeLogs);

            var id = 123;
            var request = new TimeLogRequest
            {
                ProjectId = 1,
                DeveloperId = 2,
                TimeSpent = 120,
                LogDate = DateTime.Parse("2023/02/22")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("Invalid TimeLogId", ex.Message);
        }

        [Test]
        public void UpdateAsync_ProjectClosed_ReturnException()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") } };
            AddTimeLog(timeLogs);

            var id = 123;
            var request = new TimeLogRequest
            {
                ProjectId = 2,
                DeveloperId = 2,
                TimeSpent = 120,
                LogDate = DateTime.Parse("2023/02/22")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("This project is already closed", ex.Message);
        }

        [Test]
        public void UpdateAsync_InvalidProjectId_ReturnException()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") } };
            AddTimeLog(timeLogs);

            var id = 1;
            var request = new TimeLogRequest
            {
                ProjectId = 123,
                DeveloperId = 2,
                TimeSpent = 120,
                LogDate = DateTime.Parse("2023/02/22")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("The project is not valid", ex.Message);
        }

        [Test]
        public void UpdateAsync_InvalidTimeSpent_ReturnException()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") } };
            AddTimeLog(timeLogs);

            var id = 1;
            var request = new TimeLogRequest
            {
                ProjectId = 1,
                DeveloperId = 2,
                TimeSpent = 21,
                LogDate = DateTime.Parse("2023/02/22")
            };

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.UpdateAsync(id, request));

            //Assert
            Assert.AreEqual("Time spent must be more than 30 min", ex.Message);
        }

        [Test]
        public void DeleteAsync_ValidRequest_ReturnTimeLogResponseWithId()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") } };
            AddTimeLog(timeLogs);

            var id = 1;

            // Act
            var result = _timeLogService.DeleteAsync(id).Result;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Project test 1", result.ProjectName);
            Assert.AreEqual("Developer test 1", result.DeveloperName);
            Assert.AreEqual(timeLogs[0].TimeSpent, result.TimeSpent);
            Assert.AreEqual(timeLogs[0].LogDate, result.LogDate);
        }

        [Test]
        public void DeleteAsync_InvalidId_ReturnException()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") } };
            AddTimeLog(timeLogs);

            var id = 123;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.DeleteAsync(id));

            // Assert
            Assert.AreEqual("Invalid TimeLogId", ex.Message);
        }

        [Test]
        public void GetAllAsync_ValidRequest_ReturnTimeLogsList()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/22") },
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 2,
                    TimeSpent = 60,
                    LogDate = DateTime.Parse("2023/02/23") },
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 90,
                    LogDate = DateTime.Parse("2023/02/23") } };
            AddTimeLog(timeLogs);

            const int pageIndex = 2;
            const int pageSize = 1;

            // Act
            var result = _timeLogService.GetAllAsync(pageIndex, pageSize).Result;

            // Assert
            Assert.AreEqual(pageIndex, result.PageIndex);
            Assert.AreEqual(3, result.TotalPages);
            Assert.AreEqual(1, result.Items.Count());

            var timeLog = result.Items.FirstOrDefault();
            Assert.NotNull(timeLog);
            Assert.AreEqual("Project test 1", timeLog.ProjectName);
            Assert.AreEqual("Developer test 2", timeLog.DeveloperName);
            Assert.AreEqual(timeLogs[1].TimeSpent, timeLog.TimeSpent);
            Assert.AreEqual(timeLogs[1].LogDate, timeLog.LogDate);
        }

        [Test]
        public void GetAllAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            const int pageIndex = 2;
            const int pageSize = -1;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.GetAllAsync(pageIndex, pageSize));

            // Assert
            Assert.AreEqual("Page values should not be negative", ex.Message);
        }

        [Test]
        public void GetByIdAsync_ValidRequest_ReturnTimeLog()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") },
                new TimeLog {
                    ProjectId = 2,
                    DeveloperId = 2,
                    TimeSpent = 60,
                    LogDate = DateTime.Parse("2023/02/23") }};
            AddTimeLog(timeLogs);

            const int id = 2;

            // Act
            var timeLog = _timeLogService.GetByIdAsync(id).Result;

            // Assert
            Assert.NotNull(timeLog);
            Assert.AreEqual("Project test 2", timeLog.ProjectName);
            Assert.AreEqual("Developer test 2", timeLog.DeveloperName);
            Assert.AreEqual(timeLogs[1].TimeSpent, timeLog.TimeSpent);
            Assert.AreEqual(timeLogs[1].LogDate, timeLog.LogDate);
        }

        [Test]
        public void GetByIdAsync_InvalidId_ReturnNull()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/23") },
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 2,
                    TimeSpent = 60,
                    LogDate = DateTime.Parse("2023/02/23") }};
            AddTimeLog(timeLogs);

            // Arrange
            const int id = 123;

            // Act
            var timeLog = _timeLogService.GetByIdAsync(id).Result;

            // Assert
            Assert.Null(timeLog);
        }

        [Test]
        public void SearchAsync_ValidRequest_ReturnTimeLogsList()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/22") },
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 2,
                    TimeSpent = 60,
                    LogDate = DateTime.Parse("2023/02/23") },
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 90,
                    LogDate = DateTime.Parse("2023/02/24") } };
            AddTimeLog(timeLogs);

            var request = new TimeLogSearchRequest
            {
                ProjectId = 1,
                DeveloperId = 2,
                InitialDate = DateTime.Parse("2023/02/22"),
                FinalDate = DateTime.Parse("2023/02/23")
            };

            const int pageIndex = 1;
            const int pageSize = 1;

            // Act
            var result = _timeLogService.SearchAsync(request, pageIndex, pageSize).Result;

            // Assert
            Assert.AreEqual(pageIndex, result.PageIndex);
            Assert.AreEqual(1, result.TotalPages);
            Assert.AreEqual(1, result.Items.Count());

            var timeLogResult = result.Items.FirstOrDefault();
            Assert.NotNull(timeLogResult);
            Assert.AreEqual("Project test 1", timeLogResult.ProjectName);
            Assert.AreEqual("Developer test 2", timeLogResult.DeveloperName);
            Assert.AreEqual(timeLogs[1].TimeSpent, timeLogResult.TimeSpent);
            Assert.AreEqual(timeLogs[1].LogDate, timeLogResult.LogDate);
        }

        [Test]
        public void SearchAsync_ValidRequestNoFilters_ReturnAllTimeLogsList()
        {
            // Arrange
            var timeLogs = new List<TimeLog>() {
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 30,
                    LogDate = DateTime.Parse("2023/02/22") },
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 2,
                    TimeSpent = 60,
                    LogDate = DateTime.Parse("2023/02/23") },
                new TimeLog {
                    ProjectId = 1,
                    DeveloperId = 1,
                    TimeSpent = 90,
                    LogDate = DateTime.Parse("2023/02/24") } };
            AddTimeLog(timeLogs);

            var request = new TimeLogSearchRequest
            {
                ProjectId = null,
                DeveloperId = null,
                InitialDate = null,
                FinalDate = null
            };

            const int pageIndex = 1;
            const int pageSize = 10;

            // Act
            var result = _timeLogService.SearchAsync(request, pageIndex, pageSize).Result;

            // Assert
            Assert.AreEqual(pageIndex, result.PageIndex);
            Assert.AreEqual(1, result.TotalPages);
            Assert.AreEqual(3, result.Items.Count());

            var timeLogResult = result.Items.FirstOrDefault();
            Assert.NotNull(timeLogResult);
            Assert.AreEqual("Project test 1", timeLogResult.ProjectName);
            Assert.AreEqual("Developer test 1", timeLogResult.DeveloperName);
            Assert.AreEqual(timeLogs[2].TimeSpent, timeLogResult.TimeSpent);
            Assert.AreEqual(timeLogs[2].LogDate, timeLogResult.LogDate);
        }

        [Test]
        public void SearchAsync_InvalidRequest_ReturnException()
        {
            // Arrange
            var request = new TimeLogSearchRequest
            {
                ProjectId = 1,
                DeveloperId = 2,
                InitialDate = DateTime.Parse("2023/02/22"),
                FinalDate = DateTime.Parse("2023/02/23")
            };

            const int pageIndex = -2;
            const int pageSize = 1;

            // Act
            var ex = Assert.ThrowsAsync<TimelogException>(async () => await _timeLogService.SearchAsync(request, pageIndex, pageSize));

            // Assert
            Assert.AreEqual("Page values should not be negative", ex.Message);
        }

        private void AddTimeLog(List<TimeLog> timeLog)
        {
            _context.TimeLogs.AddRange(timeLog);
            _context.SaveChanges();
        }
    }
}