using Microsoft.EntityFrameworkCore;
using Timelogger.Domain.Entities;

namespace Timelogger.Infrastructure.Context
{
    public class TimeloggerContext : DbContext
    {
        public TimeloggerContext(DbContextOptions<TimeloggerContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }
        public DbSet<ProjectStage> ProjectStages { get; set; }
    }
}
