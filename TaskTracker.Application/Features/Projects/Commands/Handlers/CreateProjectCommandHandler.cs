using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Projects.Commands.Handlers
{
    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectDto>
    {
        private readonly IProjectRepository _repository;
        private readonly IPersonRepository _personRepository;

        public CreateProjectCommandHandler(IProjectRepository repository, IPersonRepository personRepository)
        {
            _repository = repository;
            _personRepository = personRepository;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = new Project(request.Name, request.Description, request.StartDate);

            if (request.ContributorIds != null)
            {
                foreach (var contributorId in request.ContributorIds)
                {
                    var person = await _personRepository.GetByIdAsync(contributorId);
                    if (person != null)
                    {
                        project.Contributors.Add(person);
                    }
                }
            }

            await _repository.AddAsync(project);

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
    }
}
