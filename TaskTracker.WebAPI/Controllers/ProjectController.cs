using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Features.Projects.Commands;
using TaskTracker.Application.Features.Projects.Queries;
using TaskTracker.Application.Features.Projects.Dtos;

namespace TaskTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProjectCommand command)
        {
            if (command == null)
                return BadRequest("Request body is required.");

            try
            {
                ProjectDto result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the project: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectCommand command)
        {
            if (id != command.Id)
                return BadRequest("Project ID mismatch.");

            try
            {
                ProjectDto result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Project with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the project: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteProjectCommand(id));
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Project with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the project: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetProjectsQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving projects: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetProjectByIdQuery(id));
                if (result == null)
                    return NotFound($"Project with ID {id} not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the project: {ex.Message}");
            }
        }
    }
}
