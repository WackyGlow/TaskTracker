using MediatR;
using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Features.Tasks.Commands.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskItemDto>
    {
        private readonly ITaskService _taskService;

        public CreateTaskCommandHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<TaskItemDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            return await _taskService.CreateTaskAsync(request);
        }
    }
}
