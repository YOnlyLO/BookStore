using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using BookStore.DataAccess.Context;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly BookStoreDbContext _context;

    public GenreRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Genre?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Genres
            .AsNoTracking()
            .FirstOrDefaultAsync(
                genre => genre.Id == id,
                cancellationToken);

        return entity is null
            ? null
            : MapToDomain(entity);
    }

    public async Task<IReadOnlyList<Genre>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Genres
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities
            .Select(MapToDomain)
            .ToList();
    }

    public async Task AddAsync(
        Genre genre,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(genre);

        await _context.Genres.AddAsync(
            entity,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Genre genre,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(genre);

        _context.Genres.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Genres
            .FirstOrDefaultAsync(
                genre => genre.Id == id,
                cancellationToken);

        if (entity is null)
            return;

        _context.Genres.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static Genre MapToDomain(GenreEntity entity)
    {
        var result = Genre.Restore(
            entity.Id,
            entity.Name);

        return result.Value;
    }

    private static GenreEntity MapToEntity(Genre genre)
    {
        return new GenreEntity
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
}
