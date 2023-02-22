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
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        // GET v1/projects
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string sortBy, [FromQuery] string sortDirection, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var projects = await _projectService.GetAllAsync(pageIndex, pageSize, sortBy, sortDirection);

                if (projects == null || !projects.Items.Any())
                    return NotFound();

                return Ok(projects);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectById(int id)
        {
            try
            {
                var project = await _projectService.GetByIdAsync(id);

                if (project == null)
                    return NotFound();

                return Ok(project);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddProject([FromBody] ProjectRequest projectRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var response = await _projectService.AddAsync(projectRequest);

                return Ok(response);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProject([FromQuery][Required] int id, [FromBody] ProjectRequest projectRequest)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existingProject = await _projectService.UpdateAsync(id, projectRequest);

                if (existingProject == null)
                    return NotFound();

                return Ok(existingProject);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            try
            {
                var existingProject = await _projectService.DeleteAsync(id);

                if (existingProject == null)
                    return NotFound();

                return Ok(existingProject);
            }
            catch (TimelogException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
