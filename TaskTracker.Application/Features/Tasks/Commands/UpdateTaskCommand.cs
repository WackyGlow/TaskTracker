using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.Tasks.Dtos;

namespace TaskTracker.Application.Features.Tasks.Commands
{
    public class UpdateTaskCommand : IRequest<TaskItemDto>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTimeOffset DueDate { get; }
        public bool IsCompleted { get; }
        public string Category { get; }
        public int Priority { get; }
        public bool IsRecurring { get; }
        public int? RecurrenceInterval { get; }
        public string? RecurrenceUnit { get; }
        public ICollection<PersonDto> AssignedPeople { get; }

        public UpdateTaskCommand(
            Guid id,
            string name,
            string description,
            DateTimeOffset dueDate,
            bool isCompleted,
            string category,
            int priority,
            bool isRecurring,
            int? recurrenceInterval,
            string? recurrenceUnit,
            ICollection<PersonDto> assignedPeople)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id cannot be empty.", nameof(id));
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.", nameof(name));
            if (string.IsNullOrWhiteSpace(category)) throw new ArgumentException("Category is required.", nameof(category));
            if (isRecurring && (recurrenceInterval == null || string.IsNullOrWhiteSpace(recurrenceUnit)))
                throw new ArgumentException("Recurring tasks must have interval and unit defined.");

            Id = id;
            Name = name;
            Description = description;
            DueDate = dueDate;
            IsCompleted = isCompleted;
            Category = category;
            Priority = priority;
            IsRecurring = isRecurring;
            RecurrenceInterval = recurrenceInterval;
            RecurrenceUnit = recurrenceUnit;
            AssignedPeople = assignedPeople ?? new List<PersonDto>();
        }
    }
}