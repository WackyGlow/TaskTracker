using MediatR;
using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Features.Tasks.Queries.Handler
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskItemDto>>
    {
        private readonly ITaskService _taskService;

        public GetAllTasksQueryHandler(ITaskService taskService)
        { 
            _taskService = taskService;
        }

        public async Task<IEnumerable<TaskItemDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            // Call the service to retrieve all tasks
            var tasks = await _taskService.GetAllTasksAsync();

            // Return the retrieved tasks
            return tasks;
        }

    }
}
