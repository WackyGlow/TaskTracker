using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.People.Queries.Handlers
{
    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDto>
    {
        private readonly IPersonRepository _repository;

        public GetPersonByIdQueryHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<PersonDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var person = await _repository.GetByIdAsync(request.Id);
                if (person == null)
                    return null;

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
                throw new Exception($"Failed to retrieve person with ID {request.Id}.", ex);
            }
        }
    }
}