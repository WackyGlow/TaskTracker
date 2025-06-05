using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Projects.Queries.Handler
{
    public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, ProjectDto>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetProjectByIdQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<ProjectDto> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _projectRepository.GetByIdAsync(request.Id);
                if (project == null)
                    throw new KeyNotFoundException($"Project with ID {request.Id} not found.");

                return _mapper.Map<ProjectDto>(project);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to retrieve project with ID {request.Id}.", ex);
            }
        }
    }
}