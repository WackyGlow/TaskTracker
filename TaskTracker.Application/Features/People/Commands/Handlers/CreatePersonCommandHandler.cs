using MediatR;
using TaskTracker.Application.DTOs;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonDto>
    {
        private readonly IPersonService _personService;

        public CreatePersonCommandHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            return await _personService.CreatePersonAsync(request);
        }
    }
}
