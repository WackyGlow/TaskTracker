using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.People.Commands
{
    public class UpdatePersonCommand : IRequest<PersonDto>
    {
        public Guid Id { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public DateOnly DateOfBirth { get; }

        public UpdatePersonCommand(Guid id, string firstName, string lastName, DateOnly dateOfBirth)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }
    }
}