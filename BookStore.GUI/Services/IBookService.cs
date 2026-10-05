using BookStore.GUI.Models;

namespace BookStore.GUI.Services;

public interface IBookService
{
    Task<IReadOnlyList<Book>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Book> CreateAsync(
        CreateBookRequest request,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Guid id,
        UpdateBookRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
