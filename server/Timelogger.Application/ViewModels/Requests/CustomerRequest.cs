using System.ComponentModel.DataAnnotations;
using System;

namespace Timelogger.Application.ViewModels.Requests
{
    public class CustomerRequest
    {
        [Required]
        public string CustomerName { get; set; }
    }
}
