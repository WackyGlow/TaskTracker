using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.Tasks.Dtos;

namespace TaskTracker.Application.Features.Tasks.Commands
{
    public class CreateTaskCommand : IRequest<TaskItemDto>
    {
        public string Name { get; }
        public string Description { get; }
        public DateTime DueDate { get; }
        public string Category { get; }
        public int Priority { get; }
        public bool IsRecurring { get; }
        public int? RecurrenceInterval { get; }
        public string RecurrenceUnit { get; }
        public ICollection<PersonDto> AssignedPersons { get; }

        public CreateTaskCommand(
            string name,
            string description,
            DateTime dueDate,
            string category,
            int priority,
            bool isRecurring,
            int? recurrenceInterval,
            string recurrenceUnit,
            ICollection<PersonDto> assignedPersons)
        {
            Name = name;
            Description = description;
            DueDate = dueDate;
            Category = category;
            Priority = priority;
            IsRecurring = isRecurring;
            RecurrenceInterval = recurrenceInterval;
            RecurrenceUnit = recurrenceUnit;
            AssignedPersons = assignedPersons;
        }
    }
}