using AutoMapper;
using Timelogger.Application.ViewModels.Requests;
using Timelogger.Application.ViewModels.Responses;
using Timelogger.Domain.Entities;

namespace Timelogger.Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CustomerRequest, Customer>();
            CreateMap<Customer, CustomerResponse>();

            CreateMap<DeveloperRequest, Developer>();
            CreateMap<Developer, DeveloperResponse>();

            CreateMap<ProjectStageRequest, ProjectStage>();
            CreateMap<ProjectStage, ProjectStageResponse>();

            CreateMap<ProjectRequest, Project>();
            CreateMap<Project, ProjectResponse>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Customer.CustomerName))
            .ForMember(dest => dest.ProjectStageName, opt => opt.MapFrom(src => src.ProjectStage.ProjectStageName));

            CreateMap<TimeLogRequest, TimeLog>();
            CreateMap<TimeLog, TimeLogResponse>()
            .ForMember(dest => dest.TimeLogId, opt => opt.MapFrom(src => src.TimeLogId))
            .ForMember(dest => dest.ProjectName, opt => opt.MapFrom(src => src.Project.ProjectName))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.Project.Customer.CustomerName))
            .ForMember(dest => dest.DeveloperName, opt => opt.MapFrom(src => src.Developer.DeveloperName))
            .ForMember(dest => dest.DeadLine, opt => opt.MapFrom(src => src.Project.Deadline))
            .ForMember(dest => dest.LogDate, opt => opt.MapFrom(src => src.LogDate))
            .ForMember(dest => dest.TimeSpent, opt => opt.MapFrom(src => src.TimeSpent));

        }
    }
}
