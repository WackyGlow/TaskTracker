using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.People.Commands
{
    public class CreatePersonCommand : IRequest<PersonDto>
    {
        public string FirstName { get; }
        public string LastName { get; }
        public DateOnly DateOfBirth { get; }

        public CreatePersonCommand(string firstName, string lastName, DateOnly dateOfBirth)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name is required.", nameof(firstName));

            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name is required.", nameof(lastName));

            if (dateOfBirth > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Date of birth cannot be in the future.", nameof(dateOfBirth));

            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
        }
    }
}