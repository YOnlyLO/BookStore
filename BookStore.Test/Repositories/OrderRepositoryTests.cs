using BookStore.Core.Enums;
using BookStore.Core.Models;
using BookStore.DataAccess.Entities;
using BookStore.DataAccess.Repositories;
using BookStore.Test.Infrastructure;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookStore.Test.Repositories;

public class OrderRepositoryTests : RepositoryTestBase
{
    public OrderRepositoryTests(PostgresDatabaseFixture database)
        : base(database)
    {
    }

    [Fact]
    public async Task AddAsync_PersistsOrderWithItems()
    {
        var (user, solaris, eden) = await SeedUserAndBooksAsync();

        var order = Order.Create(user.Id).Value;
        order.AddItem(solaris.Id, 2, 450m);
        order.AddItem(eden.Id, 1, 380.50m);

        await using (var context = CreateContext())
        {
            await new OrderRepository(context).AddAsync(order);
        }

        var stored = await LoadOrderAsync(order.Id);

        Assert.Equal(user.Id, stored.UserId);
        Assert.Equal(OrderStatus.New, stored.Status);
        Assert.Equal(DateTimeKind.Utc, stored.CreatedAt.Kind);
        // PostgreSQL хранит время с точностью до микросекунды.
        Assert.Equal(order.CreatedAt, stored.CreatedAt, TimeSpan.FromMilliseconds(1));
        Assert.Collection(
            stored.Items.OrderBy(item => item.UnitPrice),
            item => AssertItem(item, eden.Id, 1, 380.50m),
            item => AssertItem(item, solaris.Id, 2, 450m));
    }

    [Fact]
    public async Task AddAsync_Throws_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();
        var repository = new OrderRepository(context);

        await AssertDatabaseErrorAsync(
            PostgresErrorCodes.ForeignKeyViolation,
            () => repository.AddAsync(Order.Create(Guid.NewGuid()).Value));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOrderWithItems()
    {
        var (user, solaris, eden) = await SeedUserAndBooksAsync();
        var seeded = await SeedOrderAsync(
            user.Id,
            OrderStatus.Confirmed,
            (solaris.Id, 2, 450m),
            (eden.Id, 1, 380.50m));

        await using var context = CreateContext();
        var order = await new OrderRepository(context).GetByIdAsync(seeded.Id);

        Assert.NotNull(order);
        Assert.Equal(seeded.Id, order.Id);
        Assert.Equal(user.Id, order.UserId);
        Assert.Equal(OrderStatus.Confirmed, order.Status);
        Assert.Equal(seeded.CreatedAt, order.CreatedAt);
        Assert.Equal(2, order.Items.Count);
        Assert.Equal(2 * 450m + 380.50m, order.TotalPrice);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenMissing()
    {
        await using var context = CreateContext();

        var order = await new OrderRepository(context).GetByIdAsync(Guid.NewGuid());

        Assert.Null(order);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOrdersWithItems()
    {
        var (user, solaris, eden) = await SeedUserAndBooksAsync();
        var first = await SeedOrderAsync(user.Id, items: (solaris.Id, 1, 450m));
        var second = await SeedOrderAsync(
            user.Id,
            OrderStatus.New,
            (solaris.Id, 1, 450m),
            (eden.Id, 3, 380.50m));

        await using var context = CreateContext();
        var orders = await new OrderRepository(context).GetAllAsync();

        Assert.Equal(2, orders.Count);
        Assert.Single(orders.Single(order => order.Id == first.Id).Items);
        Assert.Equal(2, orders.Single(order => order.Id == second.Id).Items.Count);
    }

    // Регрессия: новая позиция с заданным доменом Id сохранялась как UPDATE несуществующей
    // строки (DbUpdateConcurrencyException), пока для OrderItemEntity.Id не был задан
    // ValueGeneratedNever().
    [Fact]
    public async Task UpdateAsync_AddsNewItem()
    {
        var (user, solaris, _) = await SeedUserAndBooksAsync();
        var seeded = await SeedOrderAsync(user.Id);

        await UpdateOrderAsync(seeded.Id, order => order.AddItem(solaris.Id, 2, 450m));

        var stored = await LoadOrderAsync(seeded.Id);

        AssertItem(Assert.Single(stored.Items), solaris.Id, 2, 450m);
    }

    [Fact]
    public async Task UpdateAsync_IncreasesQuantity_WhenBookAlreadyInOrder()
    {
        var (user, solaris, _) = await SeedUserAndBooksAsync();
        var seeded = await SeedOrderAsync(user.Id, items: (solaris.Id, 1, 450m));
        var originalItemId = seeded.Items.Single().Id;

        await UpdateOrderAsync(seeded.Id, order => order.AddItem(solaris.Id, 2, 450m));

        var item = Assert.Single((await LoadOrderAsync(seeded.Id)).Items);

        Assert.Equal(originalItemId, item.Id);
        Assert.Equal(3, item.Quantity);
    }

    [Fact]
    public async Task UpdateAsync_RemovesItem()
    {
        var (user, solaris, eden) = await SeedUserAndBooksAsync();
        var seeded = await SeedOrderAsync(
            user.Id,
            OrderStatus.New,
            (solaris.Id, 1, 450m),
            (eden.Id, 1, 380.50m));

        await UpdateOrderAsync(seeded.Id, order => order.RemoveItem(solaris.Id));

        var stored = await LoadOrderAsync(seeded.Id);

        Assert.Equal(eden.Id, Assert.Single(stored.Items).BookId);
        Assert.Equal(1, await QueryAsync(db => db.OrderItems.CountAsync()));
    }

    [Fact]
    public async Task UpdateAsync_AddsItemAgain_AfterItWasRemoved()
    {
        var (user, solaris, _) = await SeedUserAndBooksAsync();
        var seeded = await SeedOrderAsync(user.Id, items: (solaris.Id, 1, 450m));

        await UpdateOrderAsync(seeded.Id, order => order.RemoveItem(solaris.Id));
        await UpdateOrderAsync(seeded.Id, order => order.AddItem(solaris.Id, 5, 400m));

        var item = Assert.Single((await LoadOrderAsync(seeded.Id)).Items);

        Assert.NotEqual(seeded.Items.Single().Id, item.Id);
        AssertItem(item, solaris.Id, 5, 400m);
    }

    // Удаление и повторное добавление той же книги в одном сохранении: EF должен выполнить
    // DELETE раньше INSERT, иначе сработает уникальный индекс (OrderId, BookId).
    [Fact]
    public async Task UpdateAsync_ReplacesItem_WhenBookRemovedAndAddedInOneUpdate()
    {
        var (user, solaris, _) = await SeedUserAndBooksAsync();
        var seeded = await SeedOrderAsync(user.Id, items: (solaris.Id, 1, 450m));

        await UpdateOrderAsync(seeded.Id, order => Result.Combine(
            order.RemoveItem(solaris.Id),
            order.AddItem(solaris.Id, 2, 400m)));

        var item = Assert.Single((await LoadOrderAsync(seeded.Id)).Items);

        AssertItem(item, solaris.Id, 2, 400m);
    }

    [Fact]
    public async Task UpdateAsync_PersistsStatusTransitions()
    {
        var (user, solaris, _) = await SeedUserAndBooksAsync();
        var seeded = await SeedOrderAsync(user.Id, items: (solaris.Id, 1, 450m));

        await UpdateOrderAsync(seeded.Id, order => order.Confirm());
        Assert.Equal(OrderStatus.Confirmed, (await LoadOrderAsync(seeded.Id)).Status);

        await UpdateOrderAsync(seeded.Id, order => order.Complete());
        Assert.Equal(OrderStatus.Completed, (await LoadOrderAsync(seeded.Id)).Status);
    }

    [Fact]
    public async Task UpdateAsync_DoesNothing_WhenOrderDoesNotExist()
    {
        var user = await SeedUserAsync();
        var order = Order.Create(user.Id).Value;

        await using (var context = CreateContext())
        {
            await new OrderRepository(context).UpdateAsync(order);
        }

        Assert.False(await QueryAsync(db => db.Orders.AnyAsync()));
    }

    [Fact]
    public async Task DeleteAsync_RemovesOrderWithItems()
    {
        var (user, solaris, eden) = await SeedUserAndBooksAsync();
        var seeded = await SeedOrderAsync(
            user.Id,
            OrderStatus.New,
            (solaris.Id, 1, 450m),
            (eden.Id, 1, 380.50m));

        await using (var context = CreateContext())
        {
            await new OrderRepository(context).DeleteAsync(seeded.Id);
        }

        Assert.False(await QueryAsync(db => db.Orders.AnyAsync()));
        Assert.False(await QueryAsync(db => db.OrderItems.AnyAsync()));
    }

    [Fact]
    public async Task DeleteAsync_DoesNothing_WhenOrderDoesNotExist()
    {
        var user = await SeedUserAsync();
        await SeedOrderAsync(user.Id);

        await using (var context = CreateContext())
        {
            await new OrderRepository(context).DeleteAsync(Guid.NewGuid());
        }

        Assert.Equal(1, await QueryAsync(db => db.Orders.CountAsync()));
    }

    private async Task<(UserEntity User, BookEntity Solaris, BookEntity Eden)> SeedUserAndBooksAsync()
    {
        var user = await SeedUserAsync();
        var genre = await SeedGenreAsync();
        var solaris = await SeedBookAsync(genre.Id, "Солярис", 450m);
        var eden = await SeedBookAsync(genre.Id, "Эдем", 380.50m);

        return (user, solaris, eden);
    }

    /// <summary>
    /// Повторяет сценарий сервиса: загрузить заказ, изменить доменную модель, сохранить.
    /// Каждый вызов — отдельный контекст, как отдельный HTTP-запрос.
    /// </summary>
    private async Task UpdateOrderAsync(Guid orderId, Func<Order, Result> change)
    {
        await using var context = CreateContext();
        var repository = new OrderRepository(context);

        var order = await repository.GetByIdAsync(orderId);
        Assert.NotNull(order);

        var result = change(order);
        Assert.True(result.IsSuccess, result.IsFailure ? result.Error : null);

        await repository.UpdateAsync(order);
    }

    private Task<OrderEntity> LoadOrderAsync(Guid orderId)
    {
        return QueryAsync(db => db.Orders
            .Include(order => order.Items)
            .SingleAsync(order => order.Id == orderId));
    }

    private static void AssertItem(
        OrderItemEntity item,
        Guid bookId,
        int quantity,
        decimal unitPrice)
    {
        Assert.Equal(bookId, item.BookId);
        Assert.Equal(quantity, item.Quantity);
        Assert.Equal(unitPrice, item.UnitPrice);
    }
}
