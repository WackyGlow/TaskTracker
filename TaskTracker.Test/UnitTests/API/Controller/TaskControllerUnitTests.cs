using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Features.Tasks.Commands;
using TaskTracker.Application.Features.Tasks.Queries;
using TaskTracker.WebAPI.Controllers;
using MediatR;

namespace TaskTracker.Test.UnitTests
{
    public class TasksControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TasksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOkResult_WithListOfTasks()
        {
            // Arrange
            var tasks = new List<TaskItemDto>
            {
                new TaskItemDto { Id = 1, Name = "Task1", Description = "Description1" },
                new TaskItemDto { Id = 2, Name = "Task2", Description = "Description2" }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), default)).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsOkResult_WithEmptyList()
        {
            // Arrange
            var tasks = new List<TaskItemDto>();
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), default)).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetAllTasks();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTaskById_ReturnsOkResult_WhenTaskExists()
        {
            // Arrange
            var task = new TaskItemDto { Id = 1, Name = "Task1", Description = "Description1" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), default)).ReturnsAsync(task);

            // Act
            var result = await _controller.GetTaskById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetTaskById_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), default)).ReturnsAsync((TaskItemDto)null);

            // Act
            var result = await _controller.GetTaskById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateTask_ReturnsOkResult_WhenTaskIsValid()
        {
            // Arrange
            var command = new CreateTaskCommand { Name = "Task1", Description = "Description" };
            var createdTask = new TaskItemDto { Id = 1, Name = "Task1", Description = "Description" };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(createdTask);

            // Act
            var result = await _controller.CreateTask(command);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateTask_ReturnsBadRequest_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new CreateTaskCommand { Name = "", Description = "" };

            // Act
            var result = await _controller.CreateTask(command);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTask_ReturnsOkResult_WhenTaskIsUpdated()
        {
            // Arrange
            var command = new UpdateTaskCommand { Id = 1, Name = "UpdatedTask", Description = "UpdatedDescription" };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.UpdateTask(1, command);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTask_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var command = new UpdateTaskCommand { Id = 2, Name = "UpdatedTask", Description = "UpdatedDescription" };

            // Act
            var result = await _controller.UpdateTask(1, command);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsOkResult_WhenTaskIsDeleted()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), default)).ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), default)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}