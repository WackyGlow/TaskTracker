using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
