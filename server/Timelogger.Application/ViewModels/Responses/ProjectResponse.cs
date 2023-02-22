using System.ComponentModel.DataAnnotations;
using System;

namespace Timelogger.Application.ViewModels.Responses
{
    public class ProjectResponse
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectStageId { get; set; }
        public string ProjectStageName { get; set; }
        public DateTime Deadline { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
