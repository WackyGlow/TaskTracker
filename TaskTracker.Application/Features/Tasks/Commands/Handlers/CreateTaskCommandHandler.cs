using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.Tasks.Dtos;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Enums;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Application.Features.Tasks.Commands.Handlers
{
    public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskItemDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IMapper _mapper;

        public CreateTaskCommandHandler(ITaskRepository taskRepository, IPersonRepository personRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _personRepository = personRepository;
            _mapper = mapper;
        }

        public async Task<TaskItemDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var category = Category.FromName(request.Category);

                if (!Enum.IsDefined(typeof(Priority), request.Priority))
                    throw new ArgumentException("Invalid priority value.");

                var priority = (Priority)request.Priority;

                Recurrence? recurrence = null;
                if (request.IsRecurring && request.RecurrenceInterval.HasValue && !string.IsNullOrWhiteSpace(request.RecurrenceUnit))
                {
                    var unit = Enum.Parse<RecurrenceUnit>(request.RecurrenceUnit, ignoreCase: true);
                    recurrence = new Recurrence(request.RecurrenceInterval.Value, unit);
                }

                var task = new TaskItem(
                    name: request.Name,
                    description: request.Description,
                    dueDate: request.DueDate,
                    category: category,
                    priority: priority,
                    recurrence: recurrence
                );

                foreach (var personDto in request.AssignedPersons)
                {
                    var person = await _personRepository.GetByIdAsync(personDto.Id);
                    if (person != null)
                        task.AssignedPeople.Add(person);
                }

                await _taskRepository.AddAsync(task);

                return _mapper.Map<TaskItemDto>(task);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while creating the task item.", ex);
            }
        }
    }
}