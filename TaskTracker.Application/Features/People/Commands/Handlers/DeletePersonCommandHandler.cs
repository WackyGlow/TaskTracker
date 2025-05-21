using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, PersonDto>
    {
        private readonly IPersonRepository _repository;

        public DeletePersonCommandHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<PersonDto> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var person = await _repository.GetByIdAsync(request.PersonId);
                if (person == null)
                    throw new KeyNotFoundException($"Person with ID {request.PersonId} not found.");

                await _repository.DeleteAsync(person);

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
                throw new Exception($"Failed to delete Person with ID {request.PersonId}.", ex);
            }
        }
    }
}