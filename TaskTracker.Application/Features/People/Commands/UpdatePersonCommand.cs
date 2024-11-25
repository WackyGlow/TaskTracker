using MediatR;

namespace TaskTracker.Application.Features.People.Commands
{
    public class UpdatePersonCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int Age { get; set; }
    }
}
