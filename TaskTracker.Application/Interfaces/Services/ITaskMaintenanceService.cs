namespace TaskTracker.Application.Interfaces.Services
{
    public interface ITaskMaintenanceService
    {
        /// <summary>
        /// Executes the maintenance tasks for deleting completed and overdue tasks
        /// and handling recurring tasks.
        /// </summary>
        Task PerformMaintenanceAsync(CancellationToken cancellationToken);
    }
}
