using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.People.Queries.Handlers
{
    public class GetAllPeopleQueryHandler : IRequestHandler<GetPersonsQuery, IEnumerable<PersonDto>>
    {
        private readonly IPersonRepository _repository;

        public GetAllPeopleQueryHandler(IPersonRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PersonDto>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var people = await _repository.GetAllAsync();

                return people.Select(person => new PersonDto
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Age = person.DateOfBirth.Age
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve people.", ex);
            }
        }
    }
}