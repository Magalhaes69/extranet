using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Persistence.SAP_Helps;


namespace Persistence.Seeders
{
    public sealed class SapSqlQueryUserSeeder : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<SapSqlQueryUserSeeder> _logger;

        public SapSqlQueryUserSeeder(IServiceScopeFactory scopeFactory, ILogger<SapSqlQueryUserSeeder> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var sap = scope.ServiceProvider.GetRequiredService<ISapClient>();

            // 1. SQL queries
            try
            {
                foreach (var query in SapSqlQueries.All)
                    await SeedQueryAsync(sap, query, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SAP SQL query seed failed.");
                return; // the user import depends on the query
            }

            // 2. Users from @USCT
            try
            {
                var importer = scope.ServiceProvider.GetRequiredService<SapUserImporter>();
                var invited = await importer.ImportAsync(stoppingToken);
                _logger.LogInformation("USCT import finished: {Count} invitation(s) sent.", invited);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "USCT user import failed.");
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
    }
}
