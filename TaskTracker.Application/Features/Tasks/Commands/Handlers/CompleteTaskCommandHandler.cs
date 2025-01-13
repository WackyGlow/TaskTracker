using MediatR;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Features.Tasks.Commands.Handlers
{
    public class CompleteTaskCommandHandler : IRequestHandler<CompleteTaskCommand, Unit>
    {
        private readonly ITaskService _taskService;

        public CompleteTaskCommandHandler(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task<Unit> Handle(CompleteTaskCommand request, CancellationToken cancellationToken)
        {
            await _taskService.MarkTaskAsCompletedAsync(request.TaskId);
            return Unit.Value;
        }
    }
}