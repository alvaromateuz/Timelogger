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
    public class DeveloperController : Controller
    {
        private readonly IDeveloperService _developerService;

        public DeveloperController(IDeveloperService developerService)
        {
            _developerService = developerService;
        }

        // GET v1/developers
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var developers = await _developerService.GetAllAsync(pageIndex, pageSize);

                if (developers == null || !developers.Items.Any())
                    return NotFound();

                return Ok(developers);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeveloperById(int id)
        {
            try
            {
                var developer = await _developerService.GetByIdAsync(id);

                if (developer == null)
                    return NotFound();

                return Ok(developer);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDeveloper([FromBody] DeveloperRequest developerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _developerService.AddAsync(developerRequest);

                return Ok(response);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDeveloper([FromQuery][Required] int id, [FromBody] DeveloperRequest developerRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingDeveloper = await _developerService.UpdateAsync(id, developerRequest);

                if (existingDeveloper == null)
                    return NotFound();

                return Ok(existingDeveloper);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeveloper(int id)
        {
            try
            {
                var existingDeveloper = await _developerService.DeleteAsync(id);

                if (existingDeveloper == null)
                    return NotFound();

                return Ok(existingDeveloper);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
