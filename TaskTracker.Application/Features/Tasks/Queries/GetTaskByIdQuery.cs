using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;

namespace TaskTracker.Application.Features.Tasks.Queries
{
    public class GetTaskByIdQuery : IRequest<TaskItemDto>
    {
        public Guid Id { get; }

        public GetTaskByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));
            Id = id;
        }
    }
}