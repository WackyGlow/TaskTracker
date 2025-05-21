using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonDto>
    {
        private readonly IPersonRepository _repository;

        public UpdatePersonCommandHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<PersonDto> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var person = await _repository.GetByIdAsync(request.Id);
                if (person == null)
                    throw new KeyNotFoundException($"Person with ID {request.Id} not found.");

                var dob = new DateOfBirth(request.DateOfBirth);
                person.Update(request.FirstName, request.LastName, dob);

                await _repository.UpdateAsync(person);

                return new PersonDto
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.DateOfBirth.Age
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update Person with ID {request.Id}.", ex);
            }
        }
    }
}