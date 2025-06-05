using MediatR;
using TaskTracker.Application.Features.People.Dtos;

public class GetPersonsQuery : IRequest<IEnumerable<PersonDto>>
{
    public int? PageNumber { get; init; }
    public int? PageSize { get; init; }
    public string? SortBy { get; init; }
    public bool? SortDescending { get; init; }

    public GetPersonsQuery(int? pageNumber = null, int? pageSize = null, string? sortBy = null, bool? sortDescending = null)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        SortBy = sortBy;
        SortDescending = sortDescending;
    }
}