using System.ComponentModel.DataAnnotations;
using System;

namespace Timelogger.Application.ViewModels.Responses
{
    public class DeveloperResponse
    {
        public int DeveloperId { get; set; }
        public string DeveloperName { get; set; }
    }
}
