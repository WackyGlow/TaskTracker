using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Features.Projects.Commands;
using TaskTracker.Application.Features.Projects.Queries;

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

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectCommand command)
        {
            if (id != command.Id)
                return BadRequest("Project ID mismatch.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _mediator.Send(new DeleteProjectCommand(id));
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetProjectsQuery());
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _mediator.Send(new GetProjectByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
