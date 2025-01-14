using TaskTracker.Application.Interfaces.Services;
using Microsoft.Extensions.Hosting;

namespace TaskTracker.Application.Services
{
    public class TaskMaintenanceBackgroundService : BackgroundService
    {
        private readonly ITaskMaintenanceService _taskMaintenanceService;

        public TaskMaintenanceBackgroundService(ITaskMaintenanceService taskMaintenanceService)
        {
            _taskMaintenanceService = taskMaintenanceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _taskMaintenanceService.PerformMaintenanceAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    // Log the exception (use a logging framework here)
                    Console.WriteLine($"Error in TaskMaintenanceBackgroundService: {ex.Message}");
                }

                // Wait for 24 hours
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
