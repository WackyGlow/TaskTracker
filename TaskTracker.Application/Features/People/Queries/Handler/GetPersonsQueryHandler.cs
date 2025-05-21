using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.People.Queries.Handlers
{
    public class GetPersonsQueryHandler : IRequestHandler<GetPersonsQuery, IEnumerable<PersonDto>>
    {
        private readonly IPersonService _personService;

        public GetPersonsQueryHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<IEnumerable<PersonDto>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            return await _personService.GetAllPeopleAsync();
        }
    }
}