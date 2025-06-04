using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Projects.Queries.Handler
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectsQueryHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var projects = await _projectRepository.GetAllAsync();

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                IsCompleted = p.IsCompleted,
                Contributors = p.Contributors.Select(c => new PersonDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Age = c.DateOfBirth.Age
                }).ToList()
            });
        }
    }
}
