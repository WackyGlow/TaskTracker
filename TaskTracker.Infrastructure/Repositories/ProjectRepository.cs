using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Domain.Interfaces.Repositories;
using TaskTracker.Infrastructure.Data;

namespace TaskTracker.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskTrackerDbContext _dbContext;

        public ProjectRepository(TaskTrackerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.Projects
                    .Include(p => p.Contributors)
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve Project with ID {id}.", ex);
            }
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            try
            {
                return await _dbContext.Projects
                    .Include(p => p.Contributors)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to retrieve all Projects.", ex);
            }
        }

        public async Task AddAsync(Project project)
        {
            try
            {
                if (project == null)
                    throw new ArgumentNullException(nameof(project));

                await _dbContext.Projects.AddAsync(project);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add Project.", ex);
            }
        }

        public async Task UpdateAsync(Project project)
        {
            try
            {
                if (project == null)
                    throw new ArgumentNullException(nameof(project));

                var existing = await _dbContext.Projects
                    .Include(p => p.Contributors)
                    .FirstOrDefaultAsync(p => p.Id == project.Id);

                if (existing == null)
                    throw new KeyNotFoundException($"Project with ID {project.Id} not found.");

                _dbContext.Entry(existing).CurrentValues.SetValues(project);

                // Update contributors
                existing.Contributors.Clear();
                foreach (var contributor in project.Contributors)
                {
                    existing.Contributors.Add(contributor);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update Project with ID {project?.Id}.", ex);
            }
        }

        public async Task DeleteAsync(Project project)
        {
            try
            {
                if (project == null)
                    throw new ArgumentNullException(nameof(project));

                _dbContext.Projects.Remove(project);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete Project with ID {project?.Id}.", ex);
            }
        }
    }
}