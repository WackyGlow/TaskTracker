using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.People.Commands
{
    public class DeletePersonCommand : IRequest<PersonDto>
    {
        public Guid PersonId { get; }

        public DeletePersonCommand(Guid personId)
        {
            if (personId == Guid.Empty)
                throw new ArgumentException("Person ID cannot be empty.", nameof(personId));

            PersonId = personId;
        }
    }
}