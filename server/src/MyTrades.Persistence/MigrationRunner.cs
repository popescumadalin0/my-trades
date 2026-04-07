using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MyTrades.Domain;
using Npgsql;

namespace MyTrades.Persistence;

public class MigrationRunner
{
    private readonly DapperDbContext _dbContext;
    private readonly ILogger<MigrationRunner> _logger;

    public MigrationRunner(ILogger<MigrationRunner> logger, DapperDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task RunAsync()
    {
        using var connection = _dbContext.GetConnection();

        await EnsureMigrationsTableExists(connection);

        var executedScripts = (await connection.QueryAsync<string>(
                "SELECT script_name FROM __migrations"))
            .ToHashSet();

        var assembly = typeof(DomainMarker).Assembly;

        var migrationFiles = assembly
            .GetManifestResourceNames()
            .Where(x => x.Contains("Migrations") && x.EndsWith(".sql"))
            .OrderBy(x => x);

        foreach (var resourceName in migrationFiles)
        {
            var scriptName = resourceName.Split('.').Reverse().Skip(1).First() + ".sql";

            if (executedScripts.Contains(scriptName))
                continue;

            _logger.LogInformation($"Running migration: {scriptName}");

            await using var stream = assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);

            var sql = await reader.ReadToEndAsync();

            using var transaction = connection.BeginTransaction();

            await connection.ExecuteAsync(sql, transaction: transaction);

            await connection.ExecuteAsync(
                "INSERT INTO __migrations (script_name) VALUES (@Name)",
                new { Name = scriptName },
                transaction);

            transaction.Commit();
        }
    }

    private static async Task EnsureMigrationsTableExists(IDbConnection connection)
    {
        var sql = @"
            CREATE TABLE IF NOT EXISTS __migrations (
                id SERIAL PRIMARY KEY,
                script_name VARCHAR(255) NOT NULL UNIQUE,
                executed_at TIMESTAMPTZ NOT NULL DEFAULT NOW()
            );
        ";

        await connection.ExecuteAsync(sql);
    }
}