using System.Reflection;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IProjectRepository projectRepository, IPersonRepository personRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(request.Id);
                if (project == null)
                    throw new KeyNotFoundException($"Project with ID {request.Id} not found.");

                // Update domain entity
                project.Update(
                    request.Name,
                    request.Description,
                    request.StartDate,
                    request.EndDate,
                    request.IsCompleted
                );

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

                return _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the project: {ex.Message}", ex);
            }
        }
    }
}