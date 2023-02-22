using System.ComponentModel.DataAnnotations;
using System;

namespace Timelogger.Application.ViewModels.Requests
{
    public class ProjectStageRequest
    {
        [Required]
        public string ProjectStageName { get; set; }
    }
}
