using MediatR;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Features.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<TaskItemDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Category { get; set; }
        public int Priority { get; set; }  // 1 = Low, 2 = Medium, 3 = High
        public bool IsRecurring { get; set; }
        public int? RecurrenceInterval { get; set; }
        public string RecurrenceUnit { get; set; }
    }
}
