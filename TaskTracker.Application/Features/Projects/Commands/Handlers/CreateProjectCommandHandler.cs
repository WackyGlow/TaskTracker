using AutoMapper;
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
        private readonly IMapper _mapper;


        public CreateProjectCommandHandler(IProjectRepository repository, IPersonRepository personRepository, IMapper mapper)
        {
            _repository = repository;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = new Project(request.Name, request.Description, request.StartDate);

                if (request.ContributorIds?.Any() == true)
                {
                    project.Contributors.Clear();
                    foreach (var contributorId in request.ContributorIds)
                    {
                        var person = await _personRepository.GetByIdAsync(contributorId);
                        if (person != null)
                            project.Contributors.Add(person);
                    }
                }

                await _repository.AddAsync(project);

                return _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the project.", ex);
            }
        }
    }
}