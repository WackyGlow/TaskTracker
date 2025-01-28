using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Features.Tasks.Commands;
using TaskTracker.Application.Features.Tasks.Queries;

namespace TaskTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TasksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // Get All Tasks
        [HttpGet]
        public async Task<IActionResult> GetAllTasks()
        {
            var result = await _mediator.Send(new GetAllTasksQuery());
            return Ok(result);
        }

        // Get Task By Id
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var result = await _mediator.Send(new GetTaskByIdQuery(id));
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // Create a New Task
        [HttpPost]
        public async Task<IActionResult> CreateTask(CreateTaskCommand command)
        {
            try
            {
                if (command == null || string.IsNullOrWhiteSpace(command.Name) || string.IsNullOrWhiteSpace(command.Description))
                {
                    return BadRequest("Invalid task details provided.");
                }

                var result = await _mediator.Send(command);

                // Return a generic success message or status
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the task.");
            }
        }


        // Update an Existing Task
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] UpdateTaskCommand command)
        {
            if (id != command.Id)
                return BadRequest("Task ID mismatch.");

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        // Delete a Task
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                var result = await _mediator.Send(new DeleteTaskCommand { Id = id });
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
