using TaskTracker.Application.DTOs;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Features.Tasks.Commands;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskItemDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();

            return tasks.Select(task => new TaskItemDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                Category = task.Category,
                Priority = task.Priority,
                IsRecurring = task.IsRecurring,
                RecurrenceInterval = task.RecurrenceInterval,
                RecurrenceUnit = task.RecurrenceUnit,
                AssignedPersons = task.Assignments.Select(a => new PersonDto
                {
                    Id = a.PersonId,
                    FirstName = a.Person.FirstName,
                    LastName = a.Person.LastName,
                    Age = a.Person.Age
                }).ToList()
            });
        }

        public async Task<TaskItemDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            return new TaskItemDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                Category = task.Category,
                Priority = task.Priority,
                IsRecurring = task.IsRecurring,
                RecurrenceInterval = task.RecurrenceInterval,
                RecurrenceUnit = task.RecurrenceUnit,
                AssignedPersons = task.Assignments.Select(a => new PersonDto
                {
                    Id = a.PersonId,
                    FirstName = a.Person.FirstName,
                    LastName = a.Person.LastName,
                    Age = a.Person.Age
                }).ToList()
            };
        }

        public async Task<TaskItemDto> CreateTaskAsync(CreateTaskCommand command)
        {
            var task = new TaskItem
            {
                Name = command.Name,
                Description = command.Description,
                DueDate = command.DueDate,
                IsCompleted = false,
                Category = command.Category,
                Priority = command.Priority,
                IsRecurring = command.IsRecurring,
                RecurrenceInterval = command.RecurrenceInterval,
                RecurrenceUnit = command.RecurrenceUnit,
                Assignments = command.AssignedPersons.Select(p => new TaskAssignment
                {
                    PersonId = p.Id
                }).ToList()
            };

            await _taskRepository.AddAsync(task);

            return new TaskItemDto
            {
                Id = task.Id,
                Name = task.Name,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                Category = task.Category,
                Priority = task.Priority,
                IsRecurring = task.IsRecurring,
                RecurrenceInterval = task.RecurrenceInterval,
                RecurrenceUnit = task.RecurrenceUnit,
                AssignedPersons = task.Assignments.Select(a => new PersonDto
                {
                    Id = a.PersonId,
                    FirstName = a.Person.FirstName,
                    LastName = a.Person.LastName,
                    Age = a.Person.Age
                }).ToList()
            };
        }

        public async Task UpdateTaskAsync(UpdateTaskCommand command)
        {
            var task = await _taskRepository.GetByIdAsync(command.Id);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {command.Id} not found.");

            task.Name = command.Name;
            task.Description = command.Description;
            task.DueDate = command.DueDate;
            task.IsCompleted = command.IsCompleted;
            task.Category = command.Category;
            task.Priority = command.Priority;
            task.IsRecurring = command.IsRecurring;
            task.RecurrenceInterval = command.RecurrenceInterval;
            task.RecurrenceUnit = command.RecurrenceUnit;

            // Clear and update assignments
            task.Assignments.Clear();
            task.Assignments = command.AssignedPersons.Select(p => new TaskAssignment
            {
                PersonId = p.Id,
                TaskItemId = task.Id
            }).ToList();

            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            await _taskRepository.DeleteAsync(id);
        }

        public async Task MarkTaskAsCompletedAsync(int taskId)
        {
            var taskItem = await _taskRepository.GetByIdAsync(taskId);

            if (taskItem == null)
                throw new NotFoundException($"Task with ID {taskId} not found.");

            // Mark as completed
            taskItem.IsCompleted = true;

            // Handle recurring logic
            if (taskItem.IsRecurring && taskItem.RecurrenceInterval.HasValue)
            {
                // Calculate next due date
                DateTime nextDueDate = taskItem.DueDate;

                switch (taskItem.RecurrenceUnit?.ToLower())
                {
                    case "days":
                        nextDueDate = taskItem.DueDate.AddDays(taskItem.RecurrenceInterval.Value);
                        break;
                    case "weeks":
                        nextDueDate = taskItem.DueDate.AddDays(taskItem.RecurrenceInterval.Value * 7);
                        break;
                    case "months":
                        nextDueDate = taskItem.DueDate.AddMonths(taskItem.RecurrenceInterval.Value);
                        break;
                    default:
                        throw new ArgumentException("Invalid Recurrence Unit.");
                }

                // Create the new recurring task
                var newTask = new TaskItem
                {
                    Name = taskItem.Name,
                    Description = taskItem.Description,
                    DueDate = nextDueDate,
                    IsCompleted = false,
                    Category = taskItem.Category,
                    Priority = taskItem.Priority,
                    IsRecurring = taskItem.IsRecurring,
                    RecurrenceInterval = taskItem.RecurrenceInterval,
                    RecurrenceUnit = taskItem.RecurrenceUnit,
                    Assignments = taskItem.Assignments.Select(a => new TaskAssignment
                    {
                        PersonId = a.PersonId
                    }).ToList()
                };

                await _taskRepository.AddAsync(newTask);
            }

            await _taskRepository.UpdateAsync(taskItem);
        }
    }
}