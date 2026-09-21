using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using BookStore.DataAccess.Context;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repositories;

public class BookRepository : IBookRepository
{
    private readonly BookStoreDbContext _context;

    public BookRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Book?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(
                book => book.Id == id,
                cancellationToken);

        return entity is null
            ? null
            : MapToDomain(entity);
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Books
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return entities
            .Select(MapToDomain)
            .ToList();
    }

    public async Task AddAsync(
        Book book,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(book);

        await _context.Books.AddAsync(
            entity,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Book book,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(book);

        _context.Books.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Books
            .FirstOrDefaultAsync(
                book => book.Id == id,
                cancellationToken);

        if (entity is null)
            return;

        _context.Books.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static Book MapToDomain(BookEntity entity)
    {
        var result = Book.Restore(
            entity.Id,
            entity.Title,
            entity.Author,
            entity.Description,
            entity.Price,
            entity.StockQuantity,
            entity.GenreId);

        return result.Value;
    }

    private static BookEntity MapToEntity(Book book)
    {
        return new BookEntity
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Price = book.Price,
            StockQuantity = book.StockQuantity,
            GenreId = book.GenreId
        };
    }
}
