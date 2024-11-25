using MediatR;

namespace TaskTracker.Application.Features.People.Commands
{
    public class DeletePersonCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}