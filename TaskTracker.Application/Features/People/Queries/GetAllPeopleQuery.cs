using MediatR;
using TaskTracker.Application.DTOs;

namespace TaskTracker.Application.Features.People.Queries
{
    public class GetAllPeopleQuery : IRequest<IEnumerable<PersonDto>>
    {
    }
}
