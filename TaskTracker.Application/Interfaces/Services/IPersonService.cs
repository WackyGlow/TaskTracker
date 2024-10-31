using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Features.People.Commands;

namespace TaskTracker.Domain.Interfaces.Services
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonDto>> GetAllPeopleAsync();
        Task<PersonDto> GetPersonByIdAsync(int id);
        Task<PersonDto> CreatePersonAsync(CreatePersonCommand command);
        Task UpdatePersonAsync(UpdatePersonCommand command);
        Task DeletePersonAsync(int id);
    }
}
