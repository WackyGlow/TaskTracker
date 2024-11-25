using MediatR;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Features.People.Queries
{
    public class GetPersonByIdQuery : IRequest<PersonDto>
    {
        public int Id { get; set; }
    }
}
