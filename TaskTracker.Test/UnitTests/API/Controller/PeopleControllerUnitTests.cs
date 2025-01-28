using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskTracker.Application.Features.People.Commands;
using TaskTracker.Application.Features.People.Queries;
using TaskTracker.Application.DTOs;
using TaskTracker.WebAPI.Controllers;
using Xunit;
using MediatR;

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
            var command = new CreatePersonCommand { FirstName = "John", LastName = "Doe", Age = 30 };
            var createdPerson = new PersonDto { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 };
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
            var command = new UpdatePersonCommand { Id = 1, FirstName = "John", LastName = "Doe", Age = 31 };
            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.Update(1, command);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdsDoNotMatch()
        {
            // Arrange
            var command = new UpdatePersonCommand { Id = 2, FirstName = "John", LastName = "Doe", Age = 31 };

            // Act
            var result = await _controller.Update(1, command);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenPersonIsDeleted()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeletePersonCommand>(), default)).ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeletePersonCommand>(), default)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfPeople()
        {
            // Arrange
            var people = new List<PersonDto>
            {
                new PersonDto { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 },
                new PersonDto { Id = 2, FirstName = "Jane", LastName = "Doe", Age = 25 }
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPeopleQuery>(), default)).ReturnsAsync(people);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsOkResult_WhenPersonExists()
        {
            // Arrange
            var person = new PersonDto { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonByIdQuery>(), default)).ReturnsAsync(person);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenPersonDoesNotExist()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPersonByIdQuery>(), default)).ReturnsAsync((PersonDto)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}