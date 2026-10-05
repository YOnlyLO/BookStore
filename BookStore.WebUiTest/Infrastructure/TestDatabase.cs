using BookStore.Core.Enums;
using BookStore.DataAccess.Context;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookStore.WebUiTest.Infrastructure;

/// <summary>
/// Временная база PostgreSQL для прогона UI-тестов. С ней работает тестовый экземпляр API;
/// тесты очищают её перед каждым сценарием и заполняют начальными данными напрямую через DbContext.
/// </summary>
public sealed class TestDatabase
{
    public TestDatabase()
    {
        var builder = new NpgsqlConnectionStringBuilder(UiTestSettings.ConnectionString)
        {
            Database = $"bookstore_uitest_{Guid.NewGuid():N}"
        };

        ConnectionString = builder.ConnectionString;
    }

    public string ConnectionString { get; }

    public BookStoreDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<BookStoreDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        // Конструктор BookStoreDbContext вызывает EnsureCreated — база создаётся при первом обращении.
        return new BookStoreDbContext(options);
    }

    public async Task CreateAsync()
    {
        await using var context = CreateContext();
    }

    /// <summary>
    /// Очищает все таблицы модели, чтобы каждый сценарий начинался с пустой базы.
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

    public async Task DropAsync()
    {
        await using var context = CreateContext();

        await context.Database.EnsureDeletedAsync();
    }

    // ----- Начальные данные -----

    public Task<GenreEntity> SeedGenreAsync(string name)
    {
        return SeedAsync(new GenreEntity
        {
            Id = Guid.NewGuid(),
            Name = name
        });
    }

    public Task<BookEntity> SeedBookAsync(
        GenreEntity genre,
        string title = "Солярис",
        string author = "Станислав Лем",
        decimal price = 450m,
        int stockQuantity = 10)
    {
        return SeedAsync(new BookEntity
        {
            Id = Guid.NewGuid(),
            Title = title,
            Author = author,
            Description = "Роман о планете-океане",
            Price = price,
            StockQuantity = stockQuantity,
            GenreId = genre.Id
        });
    }

    public Task<UserEntity> SeedUserAsync(
        string firstName = "Иван",
        string lastName = "Петров",
        string email = "ivan@example.com")
    {
        return SeedAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            PasswordHash = "hash"
        });
    }

    public Task<OrderEntity> SeedOrderAsync(
        UserEntity user,
        OrderStatus status = OrderStatus.New,
        params (BookEntity Book, int Quantity)[] items)
    {
        var order = new OrderEntity
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            Status = status
        };

        foreach (var (book, quantity) in items)
        {
            order.Items.Add(new OrderItemEntity
            {
                Id = Guid.NewGuid(),
                BookId = book.Id,
                Quantity = quantity,
                UnitPrice = book.Price
            });
        }

        return SeedAsync(order);
    }

    private async Task<TEntity> SeedAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await using var context = CreateContext();

        context.Add(entity);

        await context.SaveChangesAsync();

        return entity;
    }
}
