using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TaskTracker.Infrastructure.Data;
using TaskTracker.WebAPI;

namespace TaskTracker.Test.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TaskTrackerDbContext>));
                if (descriptor != null) services.Remove(descriptor);

                services.AddDbContext<TaskTrackerDbContext>(options => options.UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}"));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TaskTrackerDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
