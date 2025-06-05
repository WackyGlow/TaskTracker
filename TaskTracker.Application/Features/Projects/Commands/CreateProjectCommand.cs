using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;

namespace TaskTracker.Application.Features.Projects.Commands
{
    public class CreateProjectCommand : IRequest<ProjectDto>
    {
        public string Name { get; }
        public string Description { get; }
        public DateTimeOffset StartDate { get; }
        public ICollection<Guid>? ContributorIds { get; }

        public CreateProjectCommand(string name, string description, DateTime startDate, ICollection<Guid>? contributorIds = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Project name is required.", nameof(name));
            if (startDate == default)
                throw new ArgumentException("Start date must be set.", nameof(startDate));

            Name = name;
            Description = description;
            StartDate = startDate;
            ContributorIds = contributorIds;
        }

    }
}
