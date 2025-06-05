using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.People.Queries.Handlers
{
    public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, PersonDto>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public GetPersonByIdQueryHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PersonDto> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetByIdAsync(request.Id);
            if (person == null)
                throw new KeyNotFoundException($"Person with ID {request.Id} not found.");

            return _mapper.Map<PersonDto>(person);
        }
    }
}