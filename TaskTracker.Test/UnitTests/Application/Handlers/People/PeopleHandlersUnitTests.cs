using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using TaskTracker.Application.Features.People.Commands;
using TaskTracker.Application.Features.People.Commands.Handlers;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Application.Features.People.Queries;
using TaskTracker.Application.Features.People.Queries.Handlers;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.ValueObjects;
using Xunit;

namespace TaskTracker.Test.UnitTests.Application.Handlers.People
{
    public class PeopleHandlersUnitTests
    {
        private readonly Mock<IPersonRepository> _repoMock = new();

        [Fact]
        public async Task CreatePerson_ReturnsDto()
        {
            var command = new CreatePersonCommand("John", "Doe", DateOnly.FromDateTime(DateTime.Today.AddYears(-20)));
            var handler = new CreatePersonCommandHandler(_repoMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.FirstName.Should().Be("John");
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Person>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePerson_WhenFound_ReturnsDto()
        {
            var person = new Person(Guid.NewGuid(), "Jane", "Doe", new DateOfBirth(DateOnly.FromDateTime(DateTime.Today.AddYears(-30))));
            _repoMock.Setup(r => r.GetByIdAsync(person.Id)).ReturnsAsync(person);
            var command = new UpdatePersonCommand(person.Id, "Jane", "Smith", DateOnly.FromDateTime(DateTime.Today.AddYears(-30)));
            var handler = new UpdatePersonCommandHandler(_repoMock.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.LastName.Should().Be("Smith");
            _repoMock.Verify(r => r.UpdateAsync(person), Times.Once);
        }

        [Fact]
        public async Task UpdatePerson_WhenMissing_Throws()
        {
            var command = new UpdatePersonCommand(Guid.NewGuid(), "A", "B", DateOnly.FromDateTime(DateTime.Today.AddYears(-20)));
            _repoMock.Setup(r => r.GetByIdAsync(command.Id)).ReturnsAsync((Person?)null);
            var handler = new UpdatePersonCommandHandler(_repoMock.Object);

            await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task DeletePerson_ReturnsDto()
        {
            var person = new Person(Guid.NewGuid(), "Bob", "Doe", new DateOfBirth(DateOnly.FromDateTime(DateTime.Today.AddYears(-40))));
            _repoMock.Setup(r => r.GetByIdAsync(person.Id)).ReturnsAsync(person);
            var handler = new DeletePersonCommandHandler(_repoMock.Object);

            var result = await handler.Handle(new DeletePersonCommand(person.Id), CancellationToken.None);

            result.Id.Should().Be(person.Id);
            _repoMock.Verify(r => r.DeleteAsync(person), Times.Once);
        }

        [Fact]
        public async Task GetPersons_ReturnsDtos()
        {
            var persons = new List<Person>
            {
                new Person(Guid.NewGuid(), "A", "B", new DateOfBirth(DateOnly.FromDateTime(DateTime.Today.AddYears(-10)))),
                new Person(Guid.NewGuid(), "C", "D", new DateOfBirth(DateOnly.FromDateTime(DateTime.Today.AddYears(-20))))
            };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(persons);
            var handler = new GetAllPeopleQueryHandler(_repoMock.Object);

            var result = await handler.Handle(new GetPersonsQuery(), CancellationToken.None);

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetPersonById_ReturnsDto()
        {
            var person = new Person(Guid.NewGuid(), "A", "B", new DateOfBirth(DateOnly.FromDateTime(DateTime.Today.AddYears(-10))));
            _repoMock.Setup(r => r.GetByIdAsync(person.Id)).ReturnsAsync(person);
            var handler = new GetPersonByIdQueryHandler(_repoMock.Object);

            var result = await handler.Handle(new GetPersonByIdQuery(person.Id), CancellationToken.None);

            result!.Id.Should().Be(person.Id);
        }
    }
}

