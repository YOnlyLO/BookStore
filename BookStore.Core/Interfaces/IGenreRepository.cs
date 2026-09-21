using BookStore.Core.Models;

namespace BookStore.Core.Interfaces;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Genre>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Genre genre,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Genre genre,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
