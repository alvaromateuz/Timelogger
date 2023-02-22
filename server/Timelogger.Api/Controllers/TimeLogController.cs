using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Timelogger.Application.Exceptions;
using Timelogger.Application.ViewModels.Requests;
using Timelogger.Application.Interfaces.Services;

namespace Timelogger.Api.Controllers
{
    [Route("v1/[controller]")]
    public class TimeLogController : Controller
    {
        private readonly ITimeLogService _timeLogService;

        public TimeLogController(ITimeLogService timeLogService)
        {
            _timeLogService = timeLogService;
        }

        [HttpGet]
        [Route("search")]
        public async Task<IActionResult> Search([FromQuery] TimeLogSearchRequest searchRequest, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var timeLogs = await _timeLogService.SearchAsync(searchRequest, pageIndex, pageSize);

                if (timeLogs == null || !timeLogs.Items.Any())
                    return NotFound();

                return Ok(timeLogs);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var timeLogs = await _timeLogService.GetAllAsync(pageIndex, pageSize);

                if (timeLogs == null || !timeLogs.Items.Any())
                    return NotFound();

                return Ok(timeLogs);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeLogById(int id)
        {
            try
            {
                var timeLog = await _timeLogService.GetByIdAsync(id);

                if (timeLog == null)
                    return NotFound();

                return Ok(timeLog);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTimeLog([FromBody] TimeLogRequest timeLogRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _timeLogService.AddAsync(timeLogRequest);

                return Ok(response);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTimeLog([FromQuery][Required] int id, [FromBody] TimeLogRequest timeLogRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingTimeLog = await _timeLogService.UpdateAsync(id, timeLogRequest);

                if (existingTimeLog == null)
                    return NotFound();

                return Ok(existingTimeLog);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimeLog(int id)
        {
            try
            {
                var existingTimeLog = await _timeLogService.DeleteAsync(id);

                if (existingTimeLog == null)
                    return NotFound();

                return Ok(existingTimeLog);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
