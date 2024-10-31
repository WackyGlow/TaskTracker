using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker.Application.Services
{
    using TaskTracker.Application.DTOs;
    using TaskTracker.Application.Exceptions;
    using TaskTracker.Application.Features.Tasks.Commands;
    using TaskTracker.Domain.Entities;
    using TaskTracker.Domain.Interfaces.Repositories;
    using TaskTracker.Domain.Interfaces.Services;

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
                RecurrenceUnit = task.RecurrenceUnit
            });
        }

        public async Task<TaskItemDto> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found.");

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
                RecurrenceUnit = task.RecurrenceUnit
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
                RecurrenceUnit = command.RecurrenceUnit
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
                RecurrenceUnit = task.RecurrenceUnit
            };
        }

        public async Task UpdateTaskAsync(UpdateTaskCommand command)
        {
            var task = await _taskRepository.GetByIdAsync(command.Id);
            if (task == null) throw new NotFoundException($"Task with ID {command.Id} not found.");

            task.Name = command.Name;
            task.Description = command.Description;
            task.DueDate = command.DueDate;
            task.IsCompleted = command.IsCompleted;
            task.Category = command.Category;
            task.Priority = command.Priority;
            task.IsRecurring = command.IsRecurring;
            task.RecurrenceInterval = command.RecurrenceInterval;
            task.RecurrenceUnit = command.RecurrenceUnit;

            await _taskRepository.UpdateAsync(task);
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null) throw new NotFoundException($"Task with ID {id} not found.");

            await _taskRepository.DeleteAsync(id);
        }
    }

}
