using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskTrackerDbContext _dbContext;

        public TaskRepository(TaskTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            return await _dbContext.TaskItems
                .Include(t => t.Assignments)
                    .ThenInclude(a => a.Person) // Ensure Person is included for assignments
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _dbContext.TaskItems
                .Include(t => t.Assignments)
                    .ThenInclude(a => a.Person) // Include related Person
                .ToListAsync();
        }

        public async Task AddAsync(TaskItem taskItem)
        {
            if (taskItem == null)
                throw new ArgumentNullException(nameof(taskItem));

            await _dbContext.TaskItems.AddAsync(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            if (taskItem == null)
                throw new ArgumentNullException(nameof(taskItem));

            // Ensure the entity is tracked by the context
            var existingTask = await GetByIdAsync(taskItem.Id);
            if (existingTask == null)
                throw new KeyNotFoundException($"Task with ID {taskItem.Id} not found.");

            _dbContext.Entry(existingTask).CurrentValues.SetValues(taskItem);

            // Handle Assignments updates (if required)
            existingTask.Assignments.Clear();
            foreach (var assignment in taskItem.Assignments)
            {
                existingTask.Assignments.Add(assignment);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var taskItem = await GetByIdAsync(id);
            if (taskItem == null)
                throw new KeyNotFoundException($"Task with ID {id} not found.");

            _dbContext.TaskItems.Remove(taskItem);
            await _dbContext.SaveChangesAsync();
        }
    }
}