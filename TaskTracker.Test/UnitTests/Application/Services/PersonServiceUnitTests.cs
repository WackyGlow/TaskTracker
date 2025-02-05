using Moq;
using TaskTracker.Application.Features.People.Commands;
using TaskTracker.Application.Services;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Application.Exceptions;
using Xunit;

namespace TaskTracker.Test.UnitTests.Application.Services
{
    public class PersonServiceUnitTests
    {
        private readonly Mock<IPersonRepository> _mockPersonRepository;
        private readonly PersonService _personService;

        public PersonServiceUnitTests()
        {
            _mockPersonRepository = new Mock<IPersonRepository>();
            _personService = new PersonService(_mockPersonRepository.Object);
        }

        // Helper method to create a sample person
        private Person CreateSamplePerson(int id = 1)
        {
            return new Person { Id = id, FirstName = "John", LastName = "Doe", Age = 30 };
        }

        [Fact]
        public async Task GetAllPeopleAsync_ShouldReturnAllPeople()
        {
            // Arrange
            var people = new List<Person>
            {
                CreateSamplePerson(1),
                CreateSamplePerson(2)
            };

            _mockPersonRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(people);

            // Act
            var result = await _personService.GetAllPeopleAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetAllPeopleAsync_ShouldCallRepositoryOnce()
        {
            // Arrange
            var people = new List<Person>
            {
                CreateSamplePerson(1),
                CreateSamplePerson(2)
            };

            _mockPersonRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(people);

            // Act
            await _personService.GetAllPeopleAsync();

            // Assert
            _mockPersonRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnPerson_WhenPersonExists()
        {
            // Arrange
            var person = CreateSamplePerson(1);
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            Assert.Equal(person.Id, result.Id);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnCorrectFirstName_WhenPersonExists()
        {
            // Arrange
            var person = CreateSamplePerson(1);
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            Assert.Equal(person.FirstName, result.FirstName);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnCorrectLastName_WhenPersonExists()
        {
            // Arrange
            var person = CreateSamplePerson(1);
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            Assert.Equal(person.LastName, result.LastName);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldReturnCorrectAge_WhenPersonExists()
        {
            // Arrange
            var person = CreateSamplePerson(1);
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);

            // Act
            var result = await _personService.GetPersonByIdAsync(1);

            // Assert
            Assert.Equal(person.Age, result.Age);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldCallRepositoryOnce_WhenPersonExists()
        {
            // Arrange
            var person = CreateSamplePerson(1);
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);

            // Act
            await _personService.GetPersonByIdAsync(1);

            // Assert
            _mockPersonRepository.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetPersonByIdAsync_ShouldThrowNotFoundException_WhenPersonDoesNotExist()
        {
            // Arrange
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Person)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _personService.GetPersonByIdAsync(1));
        }

        [Fact]
        public async Task CreatePersonAsync_ShouldReturnPersonDto_WhenPersonIsCreated()
        {
            // Arrange
            var command = new CreatePersonCommand { FirstName = "John", LastName = "Doe", Age = 30 };
            var person = CreateSamplePerson(1);

            _mockPersonRepository.Setup(repo => repo.AddAsync(It.IsAny<Person>()))
                .Returns(Task.CompletedTask)
                .Callback<Person>(p => p.Id = 1);

            // Act
            var result = await _personService.CreatePersonAsync(command);

            // Assert
            Assert.Equal(person.Id, result.Id);
        }

        [Fact]
        public async Task CreatePersonAsync_ShouldCallRepositoryOnce_WhenPersonIsCreated()
        {
            // Arrange
            var command = new CreatePersonCommand { FirstName = "John", LastName = "Doe", Age = 30 };

            _mockPersonRepository.Setup(repo => repo.AddAsync(It.IsAny<Person>()))
                .Returns(Task.CompletedTask)
                .Callback<Person>(p => p.Id = 1);

            // Act
            await _personService.CreatePersonAsync(command);

            // Assert
            _mockPersonRepository.Verify(repo => repo.AddAsync(It.IsAny<Person>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldUpdateFirstName_WhenPersonExists()
        {
            // Arrange
            var command = new UpdatePersonCommand { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 };
            var person = CreateSamplePerson(1);

            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);
            _mockPersonRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

            // Act
            await _personService.UpdatePersonAsync(command);

            // Assert
            Assert.Equal(command.FirstName, person.FirstName);
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldUpdateLastName_WhenPersonExists()
        {
            // Arrange
            var command = new UpdatePersonCommand { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 };
            var person = CreateSamplePerson(1);

            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);
            _mockPersonRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

            // Act
            await _personService.UpdatePersonAsync(command);

            // Assert
            Assert.Equal(command.LastName, person.LastName);
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldUpdateAge_WhenPersonExists()
        {
            // Arrange
            var command = new UpdatePersonCommand { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 };
            var person = CreateSamplePerson(1);

            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);
            _mockPersonRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

            // Act
            await _personService.UpdatePersonAsync(command);

            // Assert
            Assert.Equal(command.Age, person.Age);
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldCallRepositoryOnce_WhenPersonExists()
        {
            // Arrange
            var command = new UpdatePersonCommand { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 };
            var person = CreateSamplePerson(1);

            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);
            _mockPersonRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Person>())).Returns(Task.CompletedTask);

            // Act
            await _personService.UpdatePersonAsync(command);

            // Assert
            _mockPersonRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Person>()), Times.Once);
        }

        [Fact]
        public async Task UpdatePersonAsync_ShouldThrowNotFoundException_WhenPersonDoesNotExist()
        {
            // Arrange
            var command = new UpdatePersonCommand { Id = 1, FirstName = "John", LastName = "Doe", Age = 30 };
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Person)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _personService.UpdatePersonAsync(command));
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldCallRepositoryOnce_WhenPersonExists()
        {
            // Arrange
            var person = CreateSamplePerson(1);
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(person);
            _mockPersonRepository.Setup(repo => repo.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            await _personService.DeletePersonAsync(1);

            // Assert
            _mockPersonRepository.Verify(repo => repo.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldThrowNotFoundException_WhenPersonDoesNotExist()
        {
            // Arrange
            _mockPersonRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Person)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => _personService.DeletePersonAsync(1));
        }
    }
}