using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Timelogger.Application.Exceptions;
using Timelogger.Application.Interfaces.Services;
using Timelogger.Application.ViewModels.Requests;

namespace Timelogger.Api.Controllers
{
    [Route("v1/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET v1/customers
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var customers = await _customerService.GetAllAsync(pageIndex, pageSize);

                if (customers == null || !customers.Items.Any())
                    return NotFound();

                return Ok(customers);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);

                if (customer == null)
                    return NotFound();

                return Ok(customer);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerRequest customerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _customerService.AddAsync(customerRequest);

                return Ok(response);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromQuery][Required] int id, [FromBody] CustomerRequest customerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingCustomer = await _customerService.UpdateAsync(id, customerRequest);

                if (existingCustomer == null)
                    return NotFound();

                return Ok(existingCustomer);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var existingCustomer = await _customerService.DeleteAsync(id);

                if (existingCustomer == null)
                    return NotFound();

                return Ok(existingCustomer);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
