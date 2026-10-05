using BookStore.GUI.Models;

namespace BookStore.GUI.Services;

public interface IGenreService
{
    Task<IReadOnlyList<Genre>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Genre> CreateAsync(
        CreateGenreRequest request,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Guid id,
        UpdateGenreRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
