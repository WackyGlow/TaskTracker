using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;

namespace TaskTracker.Application.Features.Projects.Commands
{
    public class DeleteProjectCommand : IRequest<ProjectDto>
    {
        public Guid Id { get; }

        public DeleteProjectCommand(Guid id)
        {
            Id = id;
        }
    }
}
