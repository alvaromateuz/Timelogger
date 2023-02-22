using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timelogger.Application.ViewModels.Requests
{
    public class TimeLogRequest
    {
        [Required]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Required]
        [ForeignKey("Developer")]
        public int DeveloperId { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        public int TimeSpent { get; set; }

        public string Description { get; set; }
    }
}
