using MediatR;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
    {
        private readonly IPersonService _personService;

        public DeletePersonCommandHandler(IPersonService personService)
        {
            _personService = personService;
        }

        public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            await _personService.DeletePersonAsync(request.Id);
            return Unit.Value;
        }
    }
}