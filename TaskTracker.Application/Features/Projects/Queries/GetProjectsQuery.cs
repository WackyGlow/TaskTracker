using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;

namespace TaskTracker.Application.Features.Projects.Queries
{
    public class GetProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
        public bool? IsCompleted { get; init; } // Optional filter
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public GetProjectsQuery(bool? isCompleted = null, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if (pageSize <= 0 || pageSize > 100) throw new ArgumentOutOfRangeException(nameof(pageSize));

            IsCompleted = isCompleted;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}