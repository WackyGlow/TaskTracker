using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.Projects.Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTimeOffset StartDate { get; init; }
        public DateTimeOffset? EndDate { get; init; }
        public bool IsCompleted { get; init; }
        public ICollection<PersonDto> Contributors { get; init; } = new List<PersonDto>();

        // Front-End Data
        public string Status => IsCompleted ? "Completed" : "In Progress";
    }
}
