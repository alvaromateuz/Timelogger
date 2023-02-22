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
    public class ProjectStageController : Controller
    {
        private readonly IProjectStageService _projectStageService;

        public ProjectStageController(IProjectStageService projectStageService)
        {
            _projectStageService = projectStageService;
        }

        // GET v1/projectStages
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var projectStages = await _projectStageService.GetAllAsync(pageIndex, pageSize);

                if (projectStages == null || !projectStages.Items.Any())
                    return NotFound();

                return Ok(projectStages);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectStageById(int id)
        {
            try
            {
                var projectStage = await _projectStageService.GetByIdAsync(id);

                if (projectStage == null)
                    return NotFound();

                return Ok(projectStage);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProjectStage([FromBody] ProjectStageRequest projectStageRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _projectStageService.AddAsync(projectStageRequest);

                return Ok(response);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProjectStage([FromQuery][Required] int id, [FromBody] ProjectStageRequest projectStageRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingProjectStage = await _projectStageService.UpdateAsync(id, projectStageRequest);

                if (existingProjectStage == null)
                    return NotFound();

                return Ok(existingProjectStage);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectStage(int id)
        {
            try
            {
                var existingProjectStage = await _projectStageService.DeleteAsync(id);

                if (existingProjectStage == null)
                    return NotFound();

                return Ok(existingProjectStage);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
