

using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Persistence.Seeders
{
    public sealed class SapSqlQuerySeeder : IHostedService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SapSqlQuerySeeder> _logger;

        public SapSqlQuerySeeder(IServiceScopeFactory scopeFactory, ILogger<SapSqlQuerySeeder> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var sap = scope.ServiceProvider.GetRequiredService<ISapClient>();

            try
            {
                if (!await sap.LoginAsync(cancellationToken))
                {
                    _logger.LogWarning("SAP SQL query seed skipped: login failed.");
                    return;
                }

                try
                {
                    foreach (var query in SapSqlQueries.All)
                        await SeedQueryAsync(sap, query, cancellationToken);
                }
                finally
                {
                    await sap.LogoutAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SAP SQL query seed failed.");
            }
        }
        private async Task SeedQueryAsync(ISapClient sap, SapSqlQueryDefinition query, CancellationToken ct)
        {
            var path = $"SQLQueries('{query.SqlCode}')";
            var existing = await sap.TryGetAsync<SapSqlQueryDefinition>(path, ct);

            if (existing is null)
            {
                await sap.AddAsync<SapSqlQueryDefinition>("SQLQueries", query, ct);
                _logger.LogInformation("SAP SQL query {Code} created.", query.SqlCode);
            }
            else if (!string.Equals(existing.SqlText, query.SqlText, StringComparison.Ordinal))
            {
                await sap.PatchAsync(path, new { query.SqlName, query.SqlText }, ct);
                _logger.LogInformation("SAP SQL query {Code} updated.", query.SqlCode);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}

