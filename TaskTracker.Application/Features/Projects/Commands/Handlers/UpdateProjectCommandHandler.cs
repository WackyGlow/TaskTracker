using System.Reflection;
using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Projects.Commands.Handlers
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IPersonRepository _personRepository;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository, IPersonRepository personRepository)
        {
            _projectRepository = projectRepository;
            _personRepository = personRepository;
        }

        public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {request.Id} not found.");

            // Update basic properties via reflection since setters are private
            SetProperty(project, nameof(project.Name), request.Name);
            SetProperty(project, nameof(project.Description), request.Description);
            SetProperty(project, nameof(project.StartDate), request.StartDate);

            // Completion and end date
            if (request.IsCompleted)
            {
                project.Complete(request.EndDate ?? DateTime.UtcNow);
            }
            else
            {
                SetProperty(project, nameof(project.EndDate), request.EndDate);
                SetProperty(project, nameof(project.IsCompleted), false);
            }

            // Update contributors
            project.Contributors.Clear();
            if (request.ContributorIds != null)
            {
                foreach (var contributorId in request.ContributorIds)
                {
                    var person = await _personRepository.GetByIdAsync(contributorId);
                    if (person != null)
                        project.Contributors.Add(person);
                }
            }

            await _projectRepository.UpdateAsync(project);

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                IsCompleted = project.IsCompleted,
                Contributors = project.Contributors.Select(p => new PersonDto
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Age = p.DateOfBirth.Age
                }).ToList()
            };
        }

        private static void SetProperty<T>(T target, string propertyName, object? value)
        {
            var prop = typeof(T).GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            prop?.SetValue(target, value);
        }
    }
}
