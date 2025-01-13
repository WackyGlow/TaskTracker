using MediatR;

namespace TaskTracker.Application.Features.Tasks.Commands
{
    public class CompleteTaskCommand : IRequest<Unit>
    {
        public int TaskId { get; set; }
    }
}