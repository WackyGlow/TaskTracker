using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;

namespace TaskTracker.Application.Features.Projects.Queries
{
    public class GetProjectByIdQuery : IRequest<ProjectDto>
    {
        public Guid Id { get; }

        public GetProjectByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            Id = id;
        }
    }
}