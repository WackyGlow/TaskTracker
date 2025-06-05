using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, PersonDto>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public CreatePersonCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PersonDto> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var person = new Person(Guid.NewGuid(), request.FirstName, request.LastName, new DateOfBirth(request.DateOfBirth));
            await _repository.AddAsync(person);

            return _mapper.Map<PersonDto>(person);
        }
    }
}