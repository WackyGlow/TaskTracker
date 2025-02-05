using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Features.Tasks.Commands;
using TaskTracker.Application.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Test.UnitTests.Application.Services
{
    public class TaskServiceUnitTests
    {
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly TaskService _taskService;

        public TaskServiceUnitTests()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _taskService = new TaskService(_mockTaskRepository.Object);
        }

        // Helper method to create a sample TaskItem
        private TaskItem CreateSampleTaskItem(int id = 1)
        {
            return new TaskItem
            {
                Id = id,
                Name = "Sample Task",
                Description = "Sample Description",
                DueDate = DateTime.Now.AddDays(7),
                IsCompleted = false,
                Category = "Work",
                Priority = 3, // High priority
                IsRecurring = false,
                RecurrenceInterval = null,
                RecurrenceUnit = null,
                Assignments = new List<TaskAssignment>
                {
                    new TaskAssignment
                    {
                        PersonId = 1,
                        Person = new Person { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 } // Initialize Person
                    }
                }
            };
        }

        // Helper method to create a sample TaskItemDto
        private TaskItemDto CreateSampleTaskItemDto(int id = 1)
        {
            return new TaskItemDto
            {
                Id = id,
                Name = "Sample Task",
                Description = "Sample Description",
                DueDate = DateTime.Now.AddDays(7),
                IsCompleted = false,
                Category = "Work",
                Priority = 3, // High priority
                IsRecurring = false,
                RecurrenceInterval = null,
                RecurrenceUnit = null,
                AssignedPersons = new List<PersonDto>
                {
                    new PersonDto { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 }
                }
            };
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldReturnAllTasks()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                CreateSampleTaskItem(1),
                CreateSampleTaskItem(2)
            };

            _mockTaskRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetAllTasksAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllTasksAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                CreateSampleTaskItem(1),
                CreateSampleTaskItem(2)
            };

            _mockTaskRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tasks);

            // Act
            await _taskService.GetAllTasksAsync();

            // Assert
            _mockTaskRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldReturnTask_WhenTaskExists()
        {
            // Arrange
            var task = CreateSampleTaskItem(1);
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);

            // Act
            var result = await _taskService.GetTaskByIdAsync(1);

            // Assert
            Assert.Equal(task.Id, result.Id);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldCallRepositoryOnce_WhenTaskExists()
        {
            // Arrange
            var task = CreateSampleTaskItem(1);
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);

            // Act
            await _taskService.GetTaskByIdAsync(1);

            // Assert
            _mockTaskRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetTaskByIdAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.GetTaskByIdAsync(1));
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldReturnTaskDto_WhenTaskIsCreated()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Name = "New Task",
                Description = "New Description",
                DueDate = DateTime.Now.AddDays(7),
                Category = "Work",
                Priority = 3, // High priority
                IsRecurring = false,
                RecurrenceInterval = null,
                RecurrenceUnit = null,
                AssignedPersons = new List<PersonDto> { new PersonDto { Id = 1 } }
            };

            var task = CreateSampleTaskItem(1);
            _mockTaskRepository.Setup(repo => repo.AddAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask)
                .Callback<TaskItem>(t => t.Id = 1);

            // Act
            var result = await _taskService.CreateTaskAsync(command);

            // Assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task CreateTaskAsync_ShouldCallRepositoryOnce_WhenTaskIsCreated()
        {
            // Arrange
            var command = new CreateTaskCommand
            {
                Name = "New Task",
                Description = "New Description",
                DueDate = DateTime.Now.AddDays(7),
                Category = "Work",
                Priority = 3, // High priority
                IsRecurring = false,
                RecurrenceInterval = null,
                RecurrenceUnit = null,
                AssignedPersons = new List<PersonDto>
        {
            new PersonDto { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 }
        }
            };

            // Mock the repository to simulate adding a task
            _mockTaskRepository.Setup(repo => repo.AddAsync(It.IsAny<TaskItem>()))
                .Returns(Task.CompletedTask)
                .Callback<TaskItem>(t => t.Id = 1); // Simulate the task being assigned an ID

            // Act
            await _taskService.CreateTaskAsync(command);

            // Assert
            _mockTaskRepository.Verify(repo => repo.AddAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldUpdateTaskName_WhenTaskExists()
        {
            // Arrange
            var task = CreateSampleTaskItem(1);
            var command = new UpdateTaskCommand
            {
                Id = 1,
                Name = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(14),
                IsCompleted = true,
                Category = "Personal",
                Priority = 1, // Low priority
                IsRecurring = true,
                RecurrenceInterval = 1,
                RecurrenceUnit = "days",
                AssignedPersons = new List<PersonDto> { new PersonDto { Id = 1 } }
            };

            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);
            _mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

            // Act
            await _taskService.UpdateTaskAsync(command);

            // Assert
            Assert.Equal(command.Name, task.Name);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldCallRepositoryOnce_WhenTaskExists()
        {
            // Arrange
            var task = CreateSampleTaskItem(1);
            var command = new UpdateTaskCommand
            {
                Id = 1,
                Name = "Updated Task",
                Description = "Updated Description",
                DueDate = DateTime.Now.AddDays(14),
                IsCompleted = true,
                Category = "Personal",
                Priority = 1, // Low priority
                IsRecurring = true,
                RecurrenceInterval = 1,
                RecurrenceUnit = "days",
                AssignedPersons = new List<PersonDto> { new PersonDto { Id = 1 } }
            };

            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);
            _mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

            // Act
            await _taskService.UpdateTaskAsync(command);

            // Assert
            _mockTaskRepository.Verify(repo => repo.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTaskAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            var command = new UpdateTaskCommand { Id = 1 };
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TaskItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.UpdateTaskAsync(command));
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldCallRepositoryOnce_WhenTaskExists()
        {
            // Arrange
            var task = CreateSampleTaskItem(1);
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);
            _mockTaskRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _taskService.DeleteTaskAsync(1);

            // Assert
            _mockTaskRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteTaskAsync_ShouldThrowKeyNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TaskItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _taskService.DeleteTaskAsync(1));
        }

        [Fact]
        public async Task MarkTaskAsCompletedAsync_ShouldMarkTaskAsCompleted_WhenTaskExists()
        {
            // Arrange
            var task = CreateSampleTaskItem(1);
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);
            _mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

            // Act
            await _taskService.MarkTaskAsCompletedAsync(1);

            // Assert
            Assert.True(task.IsCompleted);
        }

        [Fact]
        public async Task MarkTaskAsCompletedAsync_ShouldCallRepositoryOnce_WhenTaskExists()
        {
            // Arrange
            var task = CreateSampleTaskItem(1);
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(task);
            _mockTaskRepository.Setup(repo => repo.UpdateAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);

            // Act
            await _taskService.MarkTaskAsCompletedAsync(1);

            // Assert
            _mockTaskRepository.Verify(repo => repo.UpdateAsync(It.IsAny<TaskItem>()), Times.Once);
        }

        [Fact]
        public async Task MarkTaskAsCompletedAsync_ShouldThrowNotFoundException_WhenTaskDoesNotExist()
        {
            // Arrange
            _mockTaskRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((TaskItem)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _taskService.MarkTaskAsCompletedAsync(1));
        }
    }
}
