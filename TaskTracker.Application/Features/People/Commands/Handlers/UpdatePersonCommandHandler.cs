using MediatR;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Unit>
    {
        private readonly IPersonService _personService;

        public UpdatePersonCommandHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<Unit> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            await _personService.UpdatePersonAsync(request);
            return Unit.Value;
        }
    }
}
