using System;

namespace Timelogger.Application.ViewModels.Requests
{
    public class TimeLogSearchRequest
    {
        public int? ProjectId { get; set; }
        public int? DeveloperId { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
        public string Description { get; set; }
    }
}
