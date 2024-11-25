using MediatR;
using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Features.People.Queries.Handlers
{
    public class GetAllPeopleQueryHandler : IRequestHandler<GetAllPeopleQuery, IEnumerable<PersonDto>>
    {
        private readonly IPersonService _personService;

        public GetAllPeopleQueryHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<IEnumerable<PersonDto>> Handle(GetAllPeopleQuery request, CancellationToken cancellationToken)
        {
            return await _personService.GetAllPeopleAsync();
        }
    }
}