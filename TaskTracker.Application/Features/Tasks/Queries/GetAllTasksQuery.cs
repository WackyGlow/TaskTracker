using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;

namespace TaskTracker.Application.Features.Tasks.Queries
{
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskItemDto>>
    {
        public bool? IsCompleted { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;

        public GetAllTasksQuery(bool? isCompleted = null, int pageNumber = 1, int pageSize = 10)
        {
            if (pageNumber <= 0) throw new ArgumentOutOfRangeException(nameof(pageNumber));
            if (pageSize <= 0 || pageSize > 100) throw new ArgumentOutOfRangeException(nameof(pageSize));

            IsCompleted = isCompleted;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}