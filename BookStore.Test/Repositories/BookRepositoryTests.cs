using BookStore.Core.Models;
using BookStore.DataAccess.Repositories;
using BookStore.Test.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookStore.Test.Repositories;

public class BookRepositoryTests : RepositoryTestBase
{
    public BookRepositoryTests(PostgresDatabaseFixture database)
        : base(database)
    {
    }

    [Fact]
    public async Task AddAsync_PersistsAllFields()
    {
        var genre = await SeedGenreAsync();
        var book = Book.Create(
            "Пикник на обочине",
            "Аркадий и Борис Стругацкие",
            "Повесть о Зоне",
            499.99m,
            7,
            genre.Id).Value;

        await using (var context = CreateContext())
        {
            await new BookRepository(context).AddAsync(book);
        }

        var stored = await QueryAsync(db => db.Books.SingleAsync());

        Assert.Equal(book.Id, stored.Id);
        Assert.Equal("Пикник на обочине", stored.Title);
        Assert.Equal("Аркадий и Борис Стругацкие", stored.Author);
        Assert.Equal("Повесть о Зоне", stored.Description);
        Assert.Equal(499.99m, stored.Price);
        Assert.Equal(7, stored.StockQuantity);
        Assert.Equal(genre.Id, stored.GenreId);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenGenreDoesNotExist()
    {
        var book = Book.Create("Солярис", "Станислав Лем", null, 450m, 1, Guid.NewGuid()).Value;

        await using var context = CreateContext();
        var repository = new BookRepository(context);

        await AssertDatabaseErrorAsync(
            PostgresErrorCodes.ForeignKeyViolation,
            () => repository.AddAsync(book));
    }

    [Fact]
    public async Task GetByIdAsync_MapsAllFields()
    {
        var genre = await SeedGenreAsync();
        var seeded = await SeedBookAsync(genre.Id, "Солярис", 450m);

        await using var context = CreateContext();
        var book = await new BookRepository(context).GetByIdAsync(seeded.Id);

        Assert.NotNull(book);
        Assert.Equal(seeded.Id, book.Id);
        Assert.Equal("Солярис", book.Title);
        Assert.Equal(seeded.Author, book.Author);
        Assert.Equal(seeded.Description, book.Description);
        Assert.Equal(450m, book.Price);
        Assert.Equal(seeded.StockQuantity, book.StockQuantity);
        Assert.Equal(genre.Id, book.GenreId);
    }

    [Fact]
    public async Task GetByIdAsync_KeepsNullDescription()
    {
        var genre = await SeedGenreAsync();
        var book = Book.Create("Солярис", "Станислав Лем", null, 450m, 1, genre.Id).Value;

        await using (var context = CreateContext())
        {
            await new BookRepository(context).AddAsync(book);
        }

        await using var readContext = CreateContext();
        var stored = await new BookRepository(readContext).GetByIdAsync(book.Id);

        Assert.NotNull(stored);
        Assert.Null(stored.Description);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenMissing()
    {
        await using var context = CreateContext();

        var book = await new BookRepository(context).GetByIdAsync(Guid.NewGuid());

        Assert.Null(book);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBooks()
    {
        var genre = await SeedGenreAsync();
        var solaris = await SeedBookAsync(genre.Id, "Солярис");
        var eden = await SeedBookAsync(genre.Id, "Эдем");

        await using var context = CreateContext();
        var books = await new BookRepository(context).GetAllAsync();

        Assert.Equal(
            new[] { solaris.Id, eden.Id }.Order(),
            books.Select(book => book.Id).Order());
    }

    [Fact]
    public async Task UpdateAsync_PersistsChanges()
    {
        var fantasy = await SeedGenreAsync("Фантастика");
        var classic = await SeedGenreAsync("Классика");
        var seeded = await SeedBookAsync(fantasy.Id);

        await using (var context = CreateContext())
        {
            var repository = new BookRepository(context);
            var book = (await repository.GetByIdAsync(seeded.Id))!;

            book.ChangeTitle("Солярис (переиздание)");
            book.ChangeAuthor("С. Лем");
            book.ChangeDescription(null);
            book.ChangePrice(520.50m);
            book.ChangeGenre(classic.Id);
            book.DecreaseStock(4);

            await repository.UpdateAsync(book);
        }

        var stored = await QueryAsync(db => db.Books.SingleAsync());

        Assert.Equal("Солярис (переиздание)", stored.Title);
        Assert.Equal("С. Лем", stored.Author);
        Assert.Null(stored.Description);
        Assert.Equal(520.50m, stored.Price);
        Assert.Equal(classic.Id, stored.GenreId);
        Assert.Equal(seeded.StockQuantity - 4, stored.StockQuantity);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenBookDoesNotExist()
    {
        var genre = await SeedGenreAsync();
        var book = Book.Create("Солярис", "Станислав Лем", null, 450m, 1, genre.Id).Value;

        await using var context = CreateContext();
        var repository = new BookRepository(context);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => repository.UpdateAsync(book));
    }

    [Fact]
    public async Task DeleteAsync_RemovesBook()
    {
        var genre = await SeedGenreAsync();
        var seeded = await SeedBookAsync(genre.Id);

        await using (var context = CreateContext())
        {
            await new BookRepository(context).DeleteAsync(seeded.Id);
        }

        Assert.False(await QueryAsync(db => db.Books.AnyAsync()));
    }

    [Fact]
    public async Task DeleteAsync_DoesNothing_WhenBookDoesNotExist()
    {
        var genre = await SeedGenreAsync();
        await SeedBookAsync(genre.Id);

        await using (var context = CreateContext())
        {
            await new BookRepository(context).DeleteAsync(Guid.NewGuid());
        }

        Assert.Equal(1, await QueryAsync(db => db.Books.CountAsync()));
    }

    [Fact]
    public async Task DeleteAsync_Throws_WhenBookIsInOrder()
    {
        var genre = await SeedGenreAsync();
        var book = await SeedBookAsync(genre.Id);
        var user = await SeedUserAsync();
        await SeedOrderAsync(user.Id, items: (book.Id, 1, book.Price));

        await using var context = CreateContext();
        var repository = new BookRepository(context);

        await AssertDatabaseErrorAsync(
            PostgresErrorCodes.RestrictViolation,
            () => repository.DeleteAsync(book.Id));
    }
}
