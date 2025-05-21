using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonDto>
    {
        private readonly IPersonRepository _repository;

        public CreatePersonCommandHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = new Person(Guid.NewGuid(), request.FirstName, request.LastName, new DateOfBirth(request.DateOfBirth));
            await _repository.AddAsync(person);

            return new PersonDto
            {
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.DateOfBirth.Age
            };
        }
    }
}