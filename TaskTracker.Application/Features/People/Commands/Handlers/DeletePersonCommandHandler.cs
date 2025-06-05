using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, PersonDto>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public DeletePersonCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PersonDto> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var person = await _repository.GetByIdAsync(request.PersonId);
                if (person == null)
                    throw new KeyNotFoundException($"Person with ID {request.PersonId} not found.");

                await _repository.DeleteAsync(person);

                return _mapper.Map<PersonDto>(person);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to delete Person with ID {request.PersonId}.", ex);
            }
        }
    }
}