using TaskTracker.Domain.Entities;

namespace TaskTracker.Domain.Interfaces.Repositories
{
    public interface ITaskAssignmentRepository
    {
        Task<TaskAssignment> GetByIdAsync(int taskId, int personId);
        Task<IEnumerable<TaskAssignment>> GetAllAsync();
        Task AddAsync(TaskAssignment taskAssignment);
        Task RemoveAsync(int taskId, int personId);
    }
}
