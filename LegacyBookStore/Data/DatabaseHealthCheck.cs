using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LegacyBookStore.Data
{
    public class DatabaseHealthCheck
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseHealthCheck(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var canConnect = await dbContext.Database.CanConnectAsync(cancellationToken);

                return canConnect
                    ? HealthCheckResult.Healthy("Database connection is OK")
                    : HealthCheckResult.Unhealthy("Cannot connect to database");
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Database check failed: {ex.Message}");
            }
        }
    }
}