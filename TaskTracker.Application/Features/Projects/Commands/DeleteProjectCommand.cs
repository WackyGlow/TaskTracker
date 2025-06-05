using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;

namespace TaskTracker.Application.Features.Projects.Commands
{
    public class DeleteProjectCommand : IRequest<ProjectDto>
    {
        public Guid Id { get; }

        public DeleteProjectCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            Id = id;
        }
    }
}
