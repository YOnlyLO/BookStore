using BookStore.Core.Models;
using BookStore.DataAccess.Repositories;
using BookStore.Test.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace BookStore.Test.Repositories;

public class GenreRepositoryTests : RepositoryTestBase
{
    public GenreRepositoryTests(PostgresDatabaseFixture database)
        : base(database)
    {
    }

    [Fact]
    public async Task AddAsync_PersistsGenre()
    {
        var genre = Genre.Create("Фантастика").Value;

        await using (var context = CreateContext())
        {
            await new GenreRepository(context).AddAsync(genre);
        }

        var stored = await QueryAsync(db => db.Genres.SingleAsync());

        Assert.Equal(genre.Id, stored.Id);
        Assert.Equal("Фантастика", stored.Name);
    }

    [Fact]
    public async Task AddAsync_Throws_WhenNameAlreadyExists()
    {
        await SeedGenreAsync("Фантастика");

        await using var context = CreateContext();
        var repository = new GenreRepository(context);

        await AssertDatabaseErrorAsync(
            PostgresErrorCodes.UniqueViolation,
            () => repository.AddAsync(Genre.Create("Фантастика").Value));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsGenre_WhenExists()
    {
        var seeded = await SeedGenreAsync("Детектив");

        await using var context = CreateContext();
        var genre = await new GenreRepository(context).GetByIdAsync(seeded.Id);

        Assert.NotNull(genre);
        Assert.Equal(seeded.Id, genre.Id);
        Assert.Equal("Детектив", genre.Name);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenMissing()
    {
        await using var context = CreateContext();

        var genre = await new GenreRepository(context).GetByIdAsync(Guid.NewGuid());

        Assert.Null(genre);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllGenres()
    {
        var fantasy = await SeedGenreAsync("Фантастика");
        var detective = await SeedGenreAsync("Детектив");

        await using var context = CreateContext();
        var genres = await new GenreRepository(context).GetAllAsync();

        Assert.Equal(
            new[] { fantasy.Id, detective.Id }.Order(),
            genres.Select(genre => genre.Id).Order());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoGenres()
    {
        await using var context = CreateContext();

        var genres = await new GenreRepository(context).GetAllAsync();

        Assert.Empty(genres);
    }

    [Fact]
    public async Task UpdateAsync_PersistsRenamedGenre()
    {
        var seeded = await SeedGenreAsync("Фантастика");

        await using (var context = CreateContext())
        {
            var repository = new GenreRepository(context);
            var genre = await repository.GetByIdAsync(seeded.Id);

            genre!.Rename("Научная фантастика");
            await repository.UpdateAsync(genre);
        }

        var stored = await QueryAsync(db => db.Genres.SingleAsync());

        Assert.Equal("Научная фантастика", stored.Name);
    }

    [Fact]
    public async Task UpdateAsync_Throws_WhenGenreDoesNotExist()
    {
        await using var context = CreateContext();
        var repository = new GenreRepository(context);

        await Assert.ThrowsAsync<DbUpdateConcurrencyException>(
            () => repository.UpdateAsync(Genre.Create("Фантастика").Value));
    }

    [Fact]
    public async Task DeleteAsync_RemovesGenre()
    {
        var seeded = await SeedGenreAsync();

        await using (var context = CreateContext())
        {
            await new GenreRepository(context).DeleteAsync(seeded.Id);
        }

        Assert.False(await QueryAsync(db => db.Genres.AnyAsync()));
    }

    [Fact]
    public async Task DeleteAsync_DoesNothing_WhenGenreDoesNotExist()
    {
        await SeedGenreAsync();

        await using (var context = CreateContext())
        {
            await new GenreRepository(context).DeleteAsync(Guid.NewGuid());
        }

        Assert.Equal(1, await QueryAsync(db => db.Genres.CountAsync()));
    }

    [Fact]
    public async Task DeleteAsync_Throws_WhenGenreHasBooks()
    {
        var genre = await SeedGenreAsync();
        await SeedBookAsync(genre.Id);

        await using var context = CreateContext();
        var repository = new GenreRepository(context);

        // ON DELETE RESTRICT в PostgreSQL даёт restrict_violation (23001), а не 23503.
        await AssertDatabaseErrorAsync(
            PostgresErrorCodes.RestrictViolation,
            () => repository.DeleteAsync(genre.Id));
    }
}
