using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;

namespace TaskTracker.Application.Features.Projects.Queries
{
    public class GetProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
    }
}
