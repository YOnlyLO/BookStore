using BookStore.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookStore.Test.Infrastructure;

/// <summary>
/// Временная база PostgreSQL для одного тестового класса. Создаётся при первом обращении
/// (конструктор BookStoreDbContext вызывает EnsureCreated) и удаляется после прогона класса.
/// Сервер задаётся переменной окружения BOOKSTORE_TEST_CONNECTION; по умолчанию —
/// локальный PostgreSQL с теми же учётными данными, что в appsettings.json API.
/// </summary>
public sealed class PostgresDatabaseFixture : IAsyncLifetime
{
    public const string ConnectionStringVariable = "BOOKSTORE_TEST_CONNECTION";

    private const string DefaultConnectionString =
        "Host=localhost;Port=5432;Username=postgres;Password=1025";

    private readonly string _connectionString;

    public PostgresDatabaseFixture()
    {
        var builder = new NpgsqlConnectionStringBuilder(
            Environment.GetEnvironmentVariable(ConnectionStringVariable) ?? DefaultConnectionString)
        {
            Database = $"bookstore_test_{Guid.NewGuid():N}"
        };

        _connectionString = builder.ConnectionString;
    }

    public BookStoreDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseNpgsql(_connectionString)
            .Options;

        return new BookStoreDbContext(options);
    }

    /// <summary>
    /// Очищает все таблицы модели, чтобы каждый тест начинался с пустой базы.
    /// </summary>
    public async Task ResetAsync()
    {
        await using var context = CreateContext();

        var tables = context.Model
            .GetEntityTypes()
            .Select(entityType => $"\"{entityType.GetTableName()}\"");

        var sql = $"TRUNCATE TABLE {string.Join(", ", tables)} CASCADE";

        await context.Database.ExecuteSqlRawAsync(sql);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await using var context = CreateContext();

        await context.Database.EnsureDeletedAsync();
    }
}
