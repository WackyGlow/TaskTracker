using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskTracker.Application.Features.People.Commands;
using TaskTracker.Application.Features.People.Queries;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PeopleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePersonCommand command)
        {
            if (command == null)
                return BadRequest("Request body is required.");

            try
            {
                PersonDto result = await _mediator.Send(command);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the person: {ex.Message}");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePersonCommand command)
        {
            if (id != command.Id)
                return BadRequest("Person ID mismatch.");

            try
            {
                PersonDto result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Person with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the person: {ex.Message}");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                PersonDto result = await _mediator.Send(new DeletePersonCommand(id));
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Person with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the person: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _mediator.Send(new GetPersonsQuery());
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving people: {ex.Message}");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetPersonByIdQuery(id));
                if (result == null)
                    return NotFound($"Person with ID {id} not found.");

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the person: {ex.Message}");
            }
        }
    }
}
