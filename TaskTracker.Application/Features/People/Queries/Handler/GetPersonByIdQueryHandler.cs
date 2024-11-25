using MediatR;
using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Features.People.Queries.Handlers
{
    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDto>
    {
        private readonly IPersonService _personService;

        public GetPersonByIdQueryHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<PersonDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            return await _personService.GetPersonByIdAsync(request.Id);
        }
    }
}