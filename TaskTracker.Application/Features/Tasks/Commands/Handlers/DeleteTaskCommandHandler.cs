using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.Tasks.Commands.Handlers
{
    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, TaskItemDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;

        public DeleteTaskCommandHandler(ITaskRepository taskRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _mapper = mapper;
        }

        public async Task<TaskItemDto> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id);
                if (task == null)
                    throw new KeyNotFoundException($"Task with ID {request.Id} was not found.");

                await _taskRepository.DeleteAsync(task);

                return _mapper.Map<TaskItemDto>(task);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting task with ID {request.Id}.", ex);
            }
        }
    }
}