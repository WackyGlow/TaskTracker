using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories
{
    public class TaskAssignmentRepository : ITaskAssignmentRepository
    {
        private readonly TaskTrackerDbContext _dbContext;

        public TaskAssignmentRepository(TaskTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TaskAssignment> GetByIdAsync(int taskId, int personId)
        {
            return await _dbContext.TaskAssignments
                .Include(ta => ta.TaskItem)
                .Include(ta => ta.Person)
                .FirstOrDefaultAsync(ta => ta.TaskItemId == taskId && ta.PersonId == personId);
        }

        public async Task<IEnumerable<TaskAssignment>> GetAllAsync()
        {
            return await _dbContext.TaskAssignments
                .Include(ta => ta.TaskItem)
                .Include(ta => ta.Person)
                .ToListAsync();
        }

        public async Task AddAsync(TaskAssignment taskAssignment)
        {
            await _dbContext.TaskAssignments.AddAsync(taskAssignment);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveAsync(int taskId, int personId)
        {
            var taskAssignment = await GetByIdAsync(taskId, personId);
            if (taskAssignment != null)
            {
                _dbContext.TaskAssignments.Remove(taskAssignment);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
