﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Features.Tasks.Commands;

namespace TaskTracker.Domain.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItemDto>> GetAllTasksAsync();
        Task<TaskItemDto> GetTaskByIdAsync(int id);
        Task<TaskItemDto> CreateTaskAsync(CreateTaskCommand command);
        Task UpdateTaskAsync(UpdateTaskCommand command);
        Task DeleteTaskAsync(int id);
        Task MarkTaskAsCompletedAsync(int taskId);
    }
}