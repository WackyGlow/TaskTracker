using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;

namespace TaskTracker.Application.Features.Projects.Commands
{
    public class UpdateProjectCommand : IRequest<ProjectDto>
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public DateTimeOffset StartDate { get; }
        public DateTimeOffset? EndDate { get; }
        public bool IsCompleted { get; }
        public ICollection<Guid>? ContributorIds { get; }

        public UpdateProjectCommand(
            Guid id,
            string name,
            string description,
            DateTimeOffset startDate,
            DateTimeOffset? endDate,
            bool isCompleted,
            ICollection<Guid>? contributorIds = null)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            Id = id;
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            IsCompleted = isCompleted;
            ContributorIds = contributorIds;
        }
    }
}