using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Features.Tasks.Commands;
using TaskTracker.Application.Features.Tasks.Queries;
using TaskTracker.WebAPI.Controllers;


namespace TaskTracker.Test.UnitTests.API.Controller
{
    public class TaskControllerUnitTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TasksController _taskController;

        public TaskControllerUnitTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _taskController = new TasksController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllTasks_ReturnsNonNullResult()
        {
            // Arrange
            var expectedTasks = new List<TaskItemDto>
            {
                new TaskItemDto { Id = 1, Name = "Task 1" },
                new TaskItemDto { Id = 2, Name = "Task 2" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _taskController.GetAllTasks();

            // Assert
            result.Should().NotBeNull(); // Test 1: Ensure the result is not null
        }

        [Fact]
        public async Task GetAllTasks_ReturnsExpectedTasks()
        {
            // Arrange
            var expectedTasks = new List<TaskItemDto>
            {
                new TaskItemDto { Id = 1, Name = "Task 1" },
                new TaskItemDto { Id = 2, Name = "Task 2" }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetAllTasksQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTasks);

            // Act
            var result = await _taskController.GetAllTasks() as OkObjectResult;

            // Assert
            result!.Value.Should().BeEquivalentTo(expectedTasks); // Test 2: Compare the returned object to the expected tasks
        }

        [Fact]
        public async Task GetTaskById_ReturnsNonNullResult()
        {
            // Arrange
            var expectedTask = new TaskItemDto { Id = 1, Name = "Test Task" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTask);

            // Act
            var result = await _taskController.GetTaskById(1);

            // Assert
            result.Should().NotBeNull(); // Ensure result is not null
        }

        [Fact]
        public async Task GetTaskById_ReturnsExpectedTask()
        {
            // Arrange
            var expectedTask = new TaskItemDto { Id = 1, Name = "Test Task" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTask);

            // Act
            var result = await _taskController.GetTaskById(1) as OkObjectResult;

            // Assert
            result!.Value.Should().BeEquivalentTo(expectedTask); // Ensure result matches expected
        }

        [Fact]
        public async Task CreateTask_ReturnsNonNullResult()
        {
            // Arrange
            var command = new CreateTaskCommand { Name = "New Task" };
            var expectedTask = new TaskItemDto { Id = 1, Name = "New Task" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTask);

            // Act
            var result = await _taskController.CreateTask(command);

            // Assert
            result.Should().NotBeNull(); // Ensure result is not null
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedTask()
        {
            // Arrange
            var command = new CreateTaskCommand { Name = "New Task" };
            var expectedTask = new TaskItemDto { Id = 1, Name = "New Task" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTask);

            // Act
            var result = await _taskController.CreateTask(command) as CreatedAtActionResult;

            // Assert
            result!.Value.Should().BeEquivalentTo(expectedTask); // Ensure result matches expected
        }

        [Fact]
        public async Task UpdateTask_ReturnsNonNullResult()
        {
            // Arrange
            var command = new UpdateTaskCommand { Id = 1, Name = "Updated Task" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _taskController.UpdateTask(command.Id, command);

            // Assert
            result.Should().NotBeNull(); // Ensure result is not null
        }

        [Fact]
        public async Task UpdateTask_ReturnsNoContent()
        {
            // Arrange
            var command = new UpdateTaskCommand { Id = 1, Name = "Updated Task" };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<UpdateTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _taskController.UpdateTask( command.Id ,command);

            // Assert
            result.Should().BeOfType<NoContentResult>(); // Ensure result is NoContent
        }

        [Fact]
        public async Task DeleteTask_ReturnsNonNullResult()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _taskController.DeleteTask(1);

            // Assert
            result.Should().NotBeNull(); // Ensure result is not null
        }

        [Fact]
        public async Task DeleteTask_ReturnsNoContent()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<DeleteTaskCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            // Act
            var result = await _taskController.DeleteTask(1);

            // Assert
            result.Should().BeOfType<NoContentResult>(); // Ensure result is NoContent
        }
    }
}
