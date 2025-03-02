using System.Data;

using Npgsql;

namespace Ildvann.Importer.Repositories;

public static class DbConnection
{
    public static IDbConnection CreateConnection()
    {
        const string server = "localhost";
        const string database = "postgres";
        const string userId = "erlend";
        const string password = "postgres";

        const string connectionString = $"Host={server};Port=5432;Database={database};Username={userId};Password={password}";
        return new NpgsqlConnection(connectionString);
    }
}