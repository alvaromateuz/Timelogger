using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timelogger.Application.ViewModels.Responses
{
    public class TimeLogResponse
    {
        public int TimeLogId { get; set; }
        public string ProjectName { get; set; }
        public string CustomerName { get; set; }
        public string DeveloperName { get; set; }
        public int TimeSpent { get; set; }
        public DateTime DeadLine { get; set; }
        public DateTime LogDate { get; set; }
        public string Description { get; set; }
    }
}
