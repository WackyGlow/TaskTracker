using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.People.Queries
{
    public class GetPersonByIdQuery : IRequest<PersonDto>
    {
        public int Id { get; set; }
    }
}
