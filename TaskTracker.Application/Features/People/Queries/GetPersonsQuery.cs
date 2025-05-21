using MediatR;
using TaskTracker.Application.Features.People.Dtos;

namespace TaskTracker.Application.Features.People.Queries
{
    public class GetAllPeopleQuery : IRequest<IEnumerable<PersonDto>>
    {
    }
}
