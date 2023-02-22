using System.ComponentModel.DataAnnotations;
using System;

namespace Timelogger.Application.ViewModels.Requests
{
    public class ProjectRequest
    {
        [Required]
        public string ProjectName { get; set; }

        [Required]
        public int ProjectStageId { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        public int CustomerId { get; set; }
    }
}
