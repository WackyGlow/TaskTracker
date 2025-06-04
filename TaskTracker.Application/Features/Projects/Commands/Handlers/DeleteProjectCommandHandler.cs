using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Projects.Commands.Handlers
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, ProjectDto>
    {
        private readonly IProjectRepository _repository;

        public DeleteProjectCommandHandler(IProjectRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectDto> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await _repository.GetByIdAsync(request.Id);
            if (project == null)
                throw new KeyNotFoundException($"Project with ID {request.Id} not found.");

            await _repository.DeleteAsync(project);

            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                IsCompleted = project.IsCompleted
            };
        }
    }
}
