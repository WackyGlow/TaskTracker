﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.DTOs;
using TaskTracker.Application.Exceptions;
using TaskTracker.Application.Features.People.Commands;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Domain.Interfaces.Services;

namespace TaskTracker.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;

        public PersonService(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<IEnumerable<PersonDto>> GetAllPeopleAsync()
        {
            var people = await _personRepository.GetAllAsync();
            return people.Select(person => new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email
            });
        }

        public async Task<PersonDto> GetPersonByIdAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) throw new NotFoundException($"Person with ID {id} not found.");

            return new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email
            };
        }

        public async Task<PersonDto> CreatePersonAsync(CreatePersonCommand command)
        {
            var person = new Person
            {
                Name = command.Name,
                Email = command.Email
            };

            await _personRepository.AddAsync(person);

            return new PersonDto
            {
                Id = person.Id,
                Name = person.Name,
                Email = person.Email
            };
        }

        public async Task UpdatePersonAsync(UpdatePersonCommand command)
        {
            var person = await _personRepository.GetByIdAsync(command.Id);
            if (person == null) throw new NotFoundException($"Person with ID {command.Id} not found.");

            person.Name = command.Name;
            person.Email = command.Email;

            await _personRepository.UpdateAsync(person);
        }

        public async Task DeletePersonAsync(int id)
        {
            var person = await _personRepository.GetByIdAsync(id);
            if (person == null) throw new NotFoundException($"Person with ID {id} not found.");

            await _personRepository.DeleteAsync(id);
        }
    }
}
