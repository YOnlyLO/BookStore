using BookStore.Core.Enums;
using BookStore.DataAccess.Context;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookStore.Test.Infrastructure;

/// <summary>
/// Основа тестов репозиториев. Тесты одного класса работают с одной временной базой
/// и выполняются последовательно; перед каждым тестом таблицы очищаются.
/// Тестируемый репозиторий и проверки используют разные экземпляры DbContext,
/// поэтому проверяется состояние базы, а не кэш change tracker'а.
/// </summary>
public abstract class RepositoryTestBase : IClassFixture<PostgresDatabaseFixture>, IAsyncLifetime
{
    private readonly PostgresDatabaseFixture _database;

    protected RepositoryTestBase(PostgresDatabaseFixture database)
    {
        _database = database;
    }

    public Task InitializeAsync() => _database.ResetAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    protected BookStoreDbContext CreateContext() => _database.CreateContext();

    /// <summary>
    /// Выполняет запрос к базе в отдельном, сразу освобождаемом контексте.
    /// </summary>
    protected async Task<T> QueryAsync<T>(Func<BookStoreDbContext, Task<T>> query)
    {
        await using var context = CreateContext();

        return await query(context);
    }

    /// <summary>
    /// Ожидает ошибку сохранения, вызванную ограничением PostgreSQL с указанным SQLSTATE.
    /// </summary>
    protected static async Task AssertDatabaseErrorAsync(string sqlState, Func<Task> action)
    {
        var exception = await Assert.ThrowsAsync<DbUpdateException>(action);

        var postgresException = Assert.IsType<PostgresException>(exception.InnerException);

        Assert.Equal(sqlState, postgresException.SqlState);
    }

    // Начальные данные записываются напрямую через DbContext, минуя тестируемые репозитории.

    protected async Task<TEntity> SeedAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await using var context = CreateContext();

        context.Add(entity);

        await context.SaveChangesAsync();

        return entity;
    }

    protected Task<GenreEntity> SeedGenreAsync(string name = "Фантастика")
    {
        return SeedAsync(new GenreEntity
        {
            Id = Guid.NewGuid(),
            Name = name
        });
    }

    protected Task<BookEntity> SeedBookAsync(
        Guid genreId,
        string title = "Солярис",
        decimal price = 450m)
    {
        return SeedAsync(new BookEntity
        {
            Id = Guid.NewGuid(),
            Title = title,
            Author = "Станислав Лем",
            Description = "Роман о планете-океане",
            Price = price,
            StockQuantity = 10,
            GenreId = genreId
        });
    }

    protected Task<UserEntity> SeedUserAsync(string email = "ivan@example.com")
    {
        return SeedAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            FirstName = "Иван",
            LastName = "Петров",
            Email = email,
            PasswordHash = "hash"
        });
    }

    protected Task<OrderEntity> SeedOrderAsync(
        Guid userId,
        OrderStatus status = OrderStatus.New,
        params (Guid BookId, int Quantity, decimal UnitPrice)[] items)
    {
        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAt = new DateTime(2026, 1, 15, 10, 30, 0, DateTimeKind.Utc),
            Status = status
        };

        foreach (var (bookId, quantity, unitPrice) in items)
        {
            order.Items.Add(new OrderItemEntity
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                Quantity = quantity,
                UnitPrice = unitPrice
            });
        }

        return SeedAsync(order);
    }
}
