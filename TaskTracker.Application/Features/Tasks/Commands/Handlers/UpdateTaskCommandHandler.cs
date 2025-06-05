using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Application.Features.Tasks.Commands.Handlers
{
    public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskItemDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public UpdateTaskCommandHandler(ITaskRepository taskRepository, IPersonRepository personRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<TaskItemDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var task = await _taskRepository.GetByIdAsync(request.Id);
                if (task == null)
                    throw new KeyNotFoundException($"Task with ID {request.Id} not found.");

                if (!Enum.IsDefined(typeof(Priority), request.Priority))
                    throw new ArgumentException("Invalid priority value.");

                var category = Category.FromName(request.Category);
                var priority = (Priority)request.Priority;

                Recurrence? recurrence = null;
                if (request.IsRecurring && request.RecurrenceInterval.HasValue && !string.IsNullOrWhiteSpace(request.RecurrenceUnit))
                {
                    var unit = Enum.Parse<RecurrenceUnit>(request.RecurrenceUnit, ignoreCase: true);
                    recurrence = new Recurrence(request.RecurrenceInterval.Value, unit);
                }

                task.Update(
                    name: request.Name,
                    description: request.Description,
                    dueDate: request.DueDate,
                    category: category,
                    priority: priority,
                    recurrence: recurrence,
                    isCompleted: request.IsCompleted
                );

                task.AssignedPeople.Clear();
                foreach (var personDto in request.AssignedPeople)
                {
                    var person = await _personRepository.GetByIdAsync(personDto.Id);
                    if (person != null)
                        task.AssignedPeople.Add(person);
                }

                await _taskRepository.UpdateAsync(task);

                return _mapper.Map<TaskItemDto>(task);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to update task item with ID {request.Id}.", ex);
            }
        }

    }
}