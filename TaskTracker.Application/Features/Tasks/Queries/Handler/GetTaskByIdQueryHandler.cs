using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Tasks.Queries.Handler
{
    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskItemDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public GetTaskByIdQueryHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskItemDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id);
                if (task == null)
                    throw new KeyNotFoundException($"Task with ID {request.Id} not found.");

                return _mapper.Map<TaskItemDto>(task);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to retrieve task with ID {request.Id}.", ex);
            }
        }
    }
}