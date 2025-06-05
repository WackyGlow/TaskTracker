using AutoMapper;
using MediatR;
using TaskTracker.Application.Features.People.Dtos;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.ValueObjects;

namespace TaskTracker.Application.Features.People.Commands.Handlers
{
    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonDto>
    {
        private readonly IPersonRepository _repository;
        private readonly IMapper _mapper;

        public UpdatePersonCommandHandler(IPersonRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PersonDto> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var person = await _repository.GetByIdAsync(request.Id);
                if (person == null)
                    throw new KeyNotFoundException($"Person with ID {request.Id} not found.");

                var dob = new DateOfBirth(request.DateOfBirth);
                person.Update(request.FirstName, request.LastName, dob);

                await _repository.UpdateAsync(person);

                return _mapper.Map<PersonDto>(person);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to update Person with ID {request.Id}.", ex);
            }
        }
    }
}