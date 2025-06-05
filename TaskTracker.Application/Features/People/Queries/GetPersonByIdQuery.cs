using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.People.Queries
{
    public class GetPersonByIdQuery : IRequest<PersonDto>
    {
        public Guid Id { get; }

        public GetPersonByIdQuery(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            Id = id;
        }
    }
}