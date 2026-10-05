using BookStore.Core.Models;
using BookStore.DataAccess.Repositories;
using BookStore.Test.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookStore.Test.Repositories;

public class UserRepositoryTests : RepositoryTestBase
{
    public UserRepositoryTests(PostgresDatabaseFixture database)
        : base(database)
    {
    }

    [Fact]
    public async Task AddAsync_PersistsAllFields()
    {
        var user = User.Create("Анна", "Смирнова", "anna@example.com", "hash-1").Value;

        await using (var context = CreateContext())
        {
            await new UserRepository(context).AddAsync(user);
        }

        var stored = await QueryAsync(db => db.Users.SingleAsync());

        Assert.Equal(user.Id, stored.Id);
        Assert.Equal("Анна", stored.FirstName);
        Assert.Equal("Смирнова", stored.LastName);
        Assert.Equal("anna@example.com", stored.Email);
        Assert.Equal("hash-1", stored.PasswordHash);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenEmailAlreadyExists()
    {
        await SeedUserAsync("anna@example.com");

        await using var context = CreateContext();
        var repository = new UserRepository(context);

        await AssertDatabaseErrorAsync(
            PostgresErrorCodes.UniqueViolation,
            () => repository.AddAsync(
                User.Create("Анна", "Иванова", "anna@example.com", "hash-2").Value));
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFields()
    {
        var seeded = await SeedUserAsync("ivan@example.com");

        await using var context = CreateContext();
        var user = await new UserRepository(context).GetByIdAsync(seeded.Id);

        Assert.NotNull(user);
        Assert.Equal(seeded.Id, user.Id);
        Assert.Equal(seeded.FirstName, user.FirstName);
        Assert.Equal(seeded.LastName, user.LastName);
        Assert.Equal("ivan@example.com", user.Email);
        Assert.Equal(seeded.PasswordHash, user.PasswordHash);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenMissing()
    {
        await using var context = CreateContext();

        var user = await new UserRepository(context).GetByIdAsync(Guid.NewGuid());

        Assert.Null(user);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        var ivan = await SeedUserAsync("ivan@example.com");
        var anna = await SeedUserAsync("anna@example.com");

        await using var context = CreateContext();
        var users = await new UserRepository(context).GetAllAsync();

        Assert.Equal(
            new[] { ivan.Id, anna.Id }.Order(),
            users.Select(user => user.Id).Order());
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        var seeded = await SeedUserAsync("ivan@example.com");

        await using (var context = CreateContext())
        {
            var repository = new UserRepository(context);
            var user = (await repository.GetByIdAsync(seeded.Id))!;

            user.ChangeName("Иван", "Сидоров");
            user.ChangeEmail("sidorov@example.com");
            user.ChangePassword("new-hash");

            await repository.UpdateAsync(user);
        }

        var stored = await QueryAsync(db => db.Users.SingleAsync());

        Assert.Equal("Иван", stored.FirstName);
        Assert.Equal("Сидоров", stored.LastName);
        Assert.Equal("sidorov@example.com", stored.Email);
        Assert.Equal("new-hash", stored.PasswordHash);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenEmailBelongsToAnotherUser()
    {
        await SeedUserAsync("anna@example.com");
        var seeded = await SeedUserAsync("ivan@example.com");

        await using var context = CreateContext();
        var repository = new UserRepository(context);
        var user = (await repository.GetByIdAsync(seeded.Id))!;

        user.ChangeEmail("anna@example.com");

        await AssertDatabaseErrorAsync(
            PostgresErrorCodes.UniqueViolation,
            () => repository.UpdateAsync(user));
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenUserDoesNotExist()
    {
        await using var context = CreateContext();
        var repository = new UserRepository(context);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => repository.UpdateAsync(
                User.Create("Анна", "Смирнова", "anna@example.com", "hash").Value));
    }

    [Fact]
    public async Task DeleteAsync_RemovesUser()
    {
        var seeded = await SeedUserAsync();

        await using (var context = CreateContext())
        {
            await new UserRepository(context).DeleteAsync(seeded.Id);
        }

        Assert.False(await QueryAsync(db => db.Users.AnyAsync()));
    }

    [Fact]
    public async Task DeleteAsync_DoesNothing_WhenUserDoesNotExist()
    {
        await SeedUserAsync();

        await using (var context = CreateContext())
        {
            await new UserRepository(context).DeleteAsync(Guid.NewGuid());
        }

        Assert.Equal(1, await QueryAsync(db => db.Users.CountAsync()));
    }

    [Fact]
    public async Task DeleteAsync_Throws_WhenUserHasOrders()
    {
        var user = await SeedUserAsync();
        await SeedOrderAsync(user.Id);

        await using var context = CreateContext();
        var repository = new UserRepository(context);

        await AssertDatabaseErrorAsync(
            PostgresErrorCodes.RestrictViolation,
            () => repository.DeleteAsync(user.Id));
    }
}
