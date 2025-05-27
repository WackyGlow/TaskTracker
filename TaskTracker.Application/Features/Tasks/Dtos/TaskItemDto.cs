using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.Tasks.Dtos
{
    public class TaskItemDto
    {
        public Guid Id { get; init; }

        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime DueDate { get; init; }
        public bool IsCompleted { get; init; }

        public string Category { get; init; } = string.Empty;  // Enum as string for readability
        public string Priority { get; init; } = string.Empty;  // Enum as string for readability

        public RecurrenceDto? Recurrence { get; init; }  // Optional value object mapping

        public List<PersonDto> AssignedPeople { get; init; } = new();
    }
}