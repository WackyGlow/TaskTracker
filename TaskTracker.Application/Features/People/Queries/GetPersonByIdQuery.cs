using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.People.Queries
{
    public class GetPersonByIdQuery : IRequest<PersonDto>
    {
        public Guid Id { get; }

        public GetPersonByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}