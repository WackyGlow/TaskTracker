using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;

namespace TaskTracker.Application.Features.Tasks.Queries
{
    public class GetAllTasksQuery : IRequest<IEnumerable<TaskItemDto>>
    {
    }
}
