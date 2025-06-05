using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Projects.Commands.Handlers
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, ProjectDto>
    {
        private readonly IProjectRepository _repository;
        private readonly IMapper _mapper;

        public DeleteProjectCommandHandler(IProjectRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _repository.GetByIdAsync(request.Id);
                if (project == null)
                    throw new KeyNotFoundException($"Project with ID {request.Id} not found.");

                await _repository.DeleteAsync(project);

                return _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                // Optionally log or wrap in a custom exception if needed.
                throw new ApplicationException($"An error occurred while deleting the project: {ex.Message}", ex);
            }
        }
    }
}