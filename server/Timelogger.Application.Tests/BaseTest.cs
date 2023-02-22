using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Timelogger.Application.AutoMapper;
using Timelogger.Infrastructure.Context;

namespace Timelogger.Application.Tests
{
    public class BaseTest
    {
        protected TimeloggerContext _context;
        protected IMapper _mapper;

        [SetUp]
        public void BaseSetup()
        {
            //Setup Context
            var options = new DbContextOptionsBuilder<TimeloggerContext>()
                .UseInMemoryDatabase(databaseName: "TimeloggerTestDb")
                .Options;

            _context = new TimeloggerContext(options);

            //Setup Mapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            _mapper = config.CreateMapper();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}