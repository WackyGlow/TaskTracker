using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly TaskTrackerDbContext _dbContext;

        public PersonRepository(TaskTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            return await _dbContext.People
                .Include(p => p.Assignments) // Include related assignments if needed
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            return await _dbContext.People
                .Include(p => p.Assignments)
                .ToListAsync();
        }

        public async Task AddAsync(Person person)
        {
            await _dbContext.People.AddAsync(person);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Person person)
        {
            _dbContext.People.Update(person);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var person = await GetByIdAsync(id);
            if (person != null)
            {
                _dbContext.People.Remove(person);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}