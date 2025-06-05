using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Tasks.Queries.Handler
{
    public class GetAllTasksQueryHandler : IRequestHandler<GetAllTasksQuery, IEnumerable<TaskItemDto>>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetAllTasksQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskItemDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tasks = await _taskRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<TaskItemDto>>(tasks);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Failed to retrieve all tasks.", ex);
            }
        }
    }
}