using MediatR;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Features.People.Commands
{
    public class CreatePersonCommand : IRequest<PersonDto>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
