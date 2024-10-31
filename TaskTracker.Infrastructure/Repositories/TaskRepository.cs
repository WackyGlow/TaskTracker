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
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _dbContext.TaskItems
                .Include(t => t.Assignments)
                .ToListAsync();
        }

        public async Task AddAsync(TaskItem taskItem)
        {
            await _dbContext.TaskItems.AddAsync(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskItem taskItem)
        {
            _dbContext.TaskItems.Update(taskItem);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var taskItem = await GetByIdAsync(id);
            if (taskItem != null)
            {
                _dbContext.TaskItems.Remove(taskItem);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}