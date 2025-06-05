using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;

namespace TaskTracker.Application.Features.Tasks.Commands
{
    public class DeleteTaskCommand : IRequest<TaskItemDto>
    {
        public Guid Id { get; }

        public DeleteTaskCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            Id = id;
        }
    }
}