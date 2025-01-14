using TaskTracker.Application.Features.Tasks.Commands;
using TaskTracker.Domain.Interfaces.Services;
using MediatR;
using TaskTracker.Application.Interfaces.Services;

namespace TaskTracker.Application.Services
{
    public class TaskMaintenanceService : ITaskMaintenanceService
    {
        private readonly ITaskService _taskService;
        private readonly IMediator _mediator;

        public TaskMaintenanceService(ITaskService taskService, IMediator mediator)
        {
            _taskService = taskService;
            _mediator = mediator;
        }

        public async Task PerformMaintenanceAsync(CancellationToken cancellationToken)
        {
            // Step 1: Get all tasks
            var allTasks = await _taskService.GetAllTasksAsync();

            foreach (var task in allTasks)
            {
                // Step 2: Check if the task is completed or overdue
                if (task.IsCompleted || (DateTime.UtcNow - task.DueDate).TotalHours > 24)
                {
                    // Step 3: Handle recurring tasks
                    if (task.IsRecurring && task.RecurrenceInterval.HasValue)
                    {
                        var nextDueDate = task.DueDate;

                        switch (task.RecurrenceUnit?.ToLower())
                        {
                            case "days":
                                nextDueDate = task.DueDate.AddDays(task.RecurrenceInterval.Value);
                                break;
                            case "weeks":
                                nextDueDate = task.DueDate.AddDays(task.RecurrenceInterval.Value * 7);
                                break;
                            case "months":
                                nextDueDate = task.DueDate.AddMonths(task.RecurrenceInterval.Value);
                                break;
                        }

                        // Create the next recurring task via a command
                        var createTaskCommand = new CreateTaskCommand
                        {
                            Name = task.Name,
                            Description = task.Description,
                            DueDate = nextDueDate,
                            Category = task.Category,
                            Priority = task.Priority,
                            IsRecurring = task.IsRecurring,
                            RecurrenceInterval = task.RecurrenceInterval,
                            RecurrenceUnit = task.RecurrenceUnit,
                            AssignedPersons = task.AssignedPersons
                        };

                        await _mediator.Send(createTaskCommand, cancellationToken);
                    }

                    // Step 4: Delete the old task
                    await _taskService.DeleteTaskAsync(task.Id);
                }
            }
        }
    }
}