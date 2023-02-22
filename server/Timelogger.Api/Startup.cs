using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Timelogger.Domain.Entities;
using Microsoft.OpenApi.Models;
using Timelogger.Application.AutoMapper;
using Timelogger.Infrastructure.Context;
using Timelogger.Infrastructure.Repositories;
using Timelogger.Application.Services;
using Timelogger.Application.Interfaces.Services;
using Timelogger.Application.Interfaces.Repositories;

namespace Timelogger.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public IConfigurationRoot Configuration { get; }

        public Startup(IWebHostEnvironment env)
        {
            _environment = env;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<TimeloggerContext>(opt => opt.UseInMemoryDatabase("e-conomic interview"));
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });

            services.AddMvc(options => options.EnableEndpointRouting = false);

            if (_environment.IsDevelopment())
            {
                services.AddCors();
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Timelogger", Version = "v1" });
            });

            services.AddAutoMapper(typeof(AutoMapperProfile));

            //Application Services
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IDeveloperService, DeveloperService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<IProjectStageService, ProjectStageService>();
            services.AddTransient<ITimeLogService, TimeLogService>();

            //Repositories
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IDeveloperRepository, DeveloperRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<IProjectStageRepository, ProjectStageRepository>();
            services.AddTransient<ITimeLogRepository, TimeLogRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials());
            }

            app.UseMvc();


            var serviceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<TimeloggerContext>();

                SeedDatabase(context);
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Timelogger V1");
            });
        }

        private static void SeedDatabase(TimeloggerContext context)
        {
            //Developers
            context.Developers.AddRange(new[] { 
                new Developer { DeveloperName = "John" }, 
                new Developer { DeveloperName = "Rose" } });

            //Customers
            context.Customers.AddRange(new[] { 
                new Customer { CustomerName = "Visma" }, 
                new Customer { CustomerName = "Farfetch" } });

            //ProjectStages
            context.ProjectStages.AddRange(new[] {
                new ProjectStage { ProjectStageId = 1, ProjectStageName = "Awaiting" },
                new ProjectStage { ProjectStageId = 2, ProjectStageName = "Started" },
                new ProjectStage { ProjectStageId = 3, ProjectStageName = "Closed" } });

            //Projects
            context.Projects.AddRange(new[] {
                new Project{ ProjectName = "e-conomic Interview", CustomerId = 1, Deadline = new System.DateTime(2023, 2, 16), ProjectStageId = 2 },
                new Project{ ProjectName = "Project A", CustomerId = 2, Deadline = new System.DateTime(2023, 2, 25), ProjectStageId = 1 },
                new Project{ ProjectName = "Project B", CustomerId = 2, Deadline = new System.DateTime(2024, 4, 11), ProjectStageId = 1 },
                new Project{ ProjectName = "Project C", CustomerId = 2, Deadline = new System.DateTime(2025, 6, 30), ProjectStageId = 3 }});

            //TimeLogs
            context.TimeLogs.AddRange(new[] {
                new TimeLog { ProjectId = 1, DeveloperId = 1, LogDate = new System.DateTime(2023, 2, 1), TimeSpent = 30, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 1, LogDate = new System.DateTime(2023, 2, 2), TimeSpent = 90, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 2, LogDate = new System.DateTime(2023, 2, 3), TimeSpent = 30, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 1, LogDate = new System.DateTime(2023, 2, 3), TimeSpent = 60, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 2, LogDate = new System.DateTime(2023, 2, 4), TimeSpent = 30, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 1, LogDate = new System.DateTime(2023, 2, 4), TimeSpent = 90, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 1, LogDate = new System.DateTime(2023, 2, 4), TimeSpent = 60, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 2, LogDate = new System.DateTime(2023, 2, 5), TimeSpent = 60, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 1, LogDate = new System.DateTime(2023, 2, 5), TimeSpent = 60, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 2, LogDate = new System.DateTime(2023, 2, 6), TimeSpent = 30, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 2, LogDate = new System.DateTime(2023, 2, 6), TimeSpent = 30, Description = "Develop new feature"},
                new TimeLog { ProjectId = 1, DeveloperId = 1, LogDate = new System.DateTime(2023, 2, 6), TimeSpent = 90, Description = "Develop new feature"} });

            context.SaveChanges();
        }
    }
}