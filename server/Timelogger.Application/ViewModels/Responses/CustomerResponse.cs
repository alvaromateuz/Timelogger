using System.ComponentModel.DataAnnotations;
using System;

namespace Timelogger.Application.ViewModels.Responses
{
    public class CustomerResponse
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
