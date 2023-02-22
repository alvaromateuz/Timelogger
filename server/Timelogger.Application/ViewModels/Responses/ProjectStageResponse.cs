using System.ComponentModel.DataAnnotations;
using System;

namespace Timelogger.Application.ViewModels.Responses
{
    public class ProjectStageResponse
    {
        public int ProjectStageId { get; set; }
        public string ProjectStageName { get; set; }
    }
}
