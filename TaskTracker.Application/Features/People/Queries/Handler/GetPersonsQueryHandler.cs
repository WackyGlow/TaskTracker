using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.People.Queries.Handlers
{
    public class GetAllPeopleQueryHandler : IRequestHandler<GetPersonsQuery, IEnumerable<PersonDto>>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPeopleQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PersonDto>> Handle(GetPersonsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var people = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<PersonDto>>(people);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve people.", ex);
            }
        }
    }
}