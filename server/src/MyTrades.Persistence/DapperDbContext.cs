using System.Data;
using Npgsql;

namespace MyTrades.Persistence;

public class DapperDbContext : IDisposable
{
    private readonly string _connectionString;
    private IDbConnection _connection;

    public DapperDbContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        if (_connection == null || _connection.State != ConnectionState.Open)
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();
        }

        return _connection;
    }

    public void Dispose()
    {
        if (_connection != null && _connection.State == ConnectionState.Open)
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}