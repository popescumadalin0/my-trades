using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace MyTrades.Domain;

public class MigrationRunner
{
    private readonly string _connectionString;
    private readonly ILogger<MigrationRunner> _logger;

    public MigrationRunner(IConfiguration configuration, ILogger<MigrationRunner> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        _logger = logger;
    }

    public async Task RunAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        await EnsureMigrationsTableExists(connection);

        var executedScripts = (await connection.QueryAsync<string>(
                "SELECT script_name FROM __migrations"))
            .ToHashSet();

        var assembly = typeof(_DomainMarker).Assembly;

        var migrationFiles = assembly
            .GetManifestResourceNames()
            .Where(x => x.Contains("Migrations") && x.EndsWith(".sql"))
            .OrderBy(x => x)
            .ToList();

        foreach (var resourceName in migrationFiles)
        {
            var scriptName = resourceName.Split('.').Reverse().Skip(1).First() + ".sql";

            if (executedScripts.Contains(scriptName))
                continue;

            _logger.LogInformation($"Running migration: {scriptName}");

            using var stream = assembly.GetManifestResourceStream(resourceName)!;
            using var reader = new StreamReader(stream);

            var sql = await reader.ReadToEndAsync();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await connection.ExecuteAsync(sql, transaction: transaction);

                await connection.ExecuteAsync(
                    "INSERT INTO __migrations (script_name) VALUES (@Name)",
                    new { Name = scriptName },
                    transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }

    private static async Task EnsureMigrationsTableExists(NpgsqlConnection connection)
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