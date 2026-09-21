using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Services;

public class BookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public Task<Book?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _bookRepository.GetByIdAsync(
            id,
            cancellationToken);
    }

    public Task<IReadOnlyList<Book>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _bookRepository.GetAllAsync(
            cancellationToken);
    }

    public async Task<Result<Book>> CreateAsync(
        string title,
        string author,
        string? description,
        decimal price,
        int stockQuantity,
        Guid genreId,
        CancellationToken cancellationToken = default)
    {
        var result = Book.Create(
            title,
            author,
            description,
            price,
            stockQuantity,
            genreId);

        if (result.IsFailure)
            return result;

        await _bookRepository.AddAsync(
            result.Value,
            cancellationToken);

        return result;
    }

    public async Task<Result> UpdateAsync(
        Book book,
        CancellationToken cancellationToken = default)
    {
        var existingBook = await _bookRepository.GetByIdAsync(
            book.Id,
            cancellationToken);

        if (existingBook is null)
            return Result.Failure("Book not found.");

        await _bookRepository.UpdateAsync(
            book,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var book = await _bookRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (book is null)
            return Result.Failure("Book not found.");

        await _bookRepository.DeleteAsync(
            id,
            cancellationToken);

        return Result.Success();
    }
}
