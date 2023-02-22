using System.ComponentModel.DataAnnotations;
using System;

namespace Timelogger.Application.ViewModels.Requests
{
    public class DeveloperRequest
    {
        [Required]
        public string DeveloperName { get; set; }
    }
}
