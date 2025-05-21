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

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            try
            {
                return await _dbContext.TaskItems
                    .Include(t => t.AssignedPeople)
                    .FirstOrDefaultAsync(t => t.Id == id);
            }
            catch (Exception ex)
            {
                // Log or rethrow depending on your design
                throw new Exception($"Failed to retrieve TaskItem with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            try
            {
                return await _dbContext.TaskItems
                    .Include(t => t.AssignedPeople)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve all TaskItems.", ex);
            }
        }

        public async Task AddAsync(TaskItem taskItem)
        {
            try
            {
                if (taskItem == null)
                    throw new ArgumentNullException(nameof(taskItem));

                await _dbContext.TaskItems.AddAsync(taskItem);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add TaskItem.", ex);
            }
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            try
            {
                if (taskItem == null)
                    throw new ArgumentNullException(nameof(taskItem));

                var existing = await _dbContext.TaskItems
                    .Include(t => t.AssignedPeople)
                    .FirstOrDefaultAsync(t => t.Id == taskItem.Id);

                if (existing == null)
                    throw new KeyNotFoundException($"Task with ID {taskItem.Id} not found.");

                _dbContext.Entry(existing).CurrentValues.SetValues(taskItem);

                existing.AssignedPeople.Clear();
                foreach (var person in taskItem.AssignedPeople)
                {
                    existing.AssignedPeople.Add(person);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update TaskItem with ID {taskItem?.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var taskItem = await _dbContext.TaskItems.FindAsync(id);
                if (taskItem == null)
                    throw new KeyNotFoundException($"Task with ID {id} not found.");

                _dbContext.TaskItems.Remove(taskItem);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete TaskItem with ID {id}.", ex);
            }
        }
    }
}