using MediatR;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Features.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest<Unit>
    {
        public int Id { get; set; }
    }
}
