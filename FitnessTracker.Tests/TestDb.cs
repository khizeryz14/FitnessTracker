using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class TestDb : IDisposable
{
    private readonly SqliteConnection _connection;
    public AppDbContext Context { get; }

    public TestDb()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();   // in-memory SQLite dies when the connection closes

        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options;
        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();   // builds the schema and applies HasData seeds
    }

    public void Dispose()
    {
        Context.Dispose();
        _connection.Dispose();
    }
}