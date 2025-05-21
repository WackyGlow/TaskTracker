using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskTracker.Application.Features.People.Commands;
using TaskTracker.Application.Features.People.Queries;
using TaskTracker.WebAPI.Controllers;
using Xunit;
using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Test.UnitTests.API.Controller
{
    public class PeopleControllerUnitTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PeopleController _controller;

        public PeopleControllerUnitTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PeopleController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ReturnsCreatedResult_WhenCommandIsValid()
        {
            // Arrange
            var command = new CreatePersonCommand("John", "Doe", DateOnly.FromDateTime(DateTime.Today.AddYears(-30)));
            var createdPerson = new PersonDto { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Age = 30 };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(createdPerson);

            // Act
            var result = await _controller.Create(command);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            Assert.Equal(createdPerson, createdResult.Value);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenCommandIsInvalid()
        {
            // Act
            var result = await _controller.Create(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenPersonIsUpdated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new UpdatePersonCommand(id, "John", "Doe", DateOnly.FromDateTime(DateTime.Today.AddYears(-31)));
            var updatedPerson = new PersonDto { Id = id, FirstName = "John", LastName = "Doe", Age = 31 };

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(updatedPerson);

            // Act
            var result = await _controller.Update(id, command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedPerson, okResult.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var command = new UpdatePersonCommand(Guid.NewGuid(), "John", "Doe", DateOnly.FromDateTime(DateTime.Today.AddYears(-31)));

            // Act
            var result = await _controller.Update(Guid.NewGuid(), command);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenPersonIsDeleted()
        {
            // Arrange
            var deleted = new PersonDto { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Age = 30 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeletePersonCommand>(), default)).ReturnsAsync(deleted);

            // Act
            var result = await _controller.Delete(deleted.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(deleted, okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeletePersonCommand>(), default)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfPeople()
        {
            // Arrange
            var people = new List<PersonDto>
            {
                new PersonDto { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Age = 30 },
                new PersonDto { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", Age = 25 }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonsQuery>(), default)).ReturnsAsync(people);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(people, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenPersonExists()
        {
            // Arrange
            var person = new PersonDto { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe", Age = 30 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonByIdQuery>(), default)).ReturnsAsync(person);

            // Act
            var result = await _controller.GetById(person.Id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(person, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonByIdQuery>(), default)).ReturnsAsync((PersonDto?)null);

            // Act
            var result = await _controller.GetById(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}