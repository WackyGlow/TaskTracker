using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.Projects.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Projects.Queries.Handler
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, IEnumerable<ProjectDto>>
    {
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public GetProjectsQueryHandler(IProjectRepository projectRepository, IMapper mapper)
        {
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var allProjects = await _projectRepository.GetAllAsync();

                // Optional filtering
                if (request.IsCompleted.HasValue)
                {
                    allProjects = allProjects
                        .Where(p => p.IsCompleted == request.IsCompleted.Value)
                        .ToList();
                }

                // Pagination
                var paged = allProjects
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize);

                return _mapper.Map<IEnumerable<ProjectDto>>(paged);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to retrieve projects.", ex);
            }
        }
    }
}