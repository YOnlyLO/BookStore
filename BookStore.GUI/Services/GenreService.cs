using BookStore.GUI.Models;

namespace BookStore.GUI.Services;

public sealed class GenreService : IGenreService
{
    private const string Resource = "api/genres";

    private readonly ApiClient _apiClient;

    public GenreService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyList<Genre>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _apiClient.GetAsync<IReadOnlyList<Genre>>(
            Resource,
            cancellationToken);
    }

    public Task<Genre> CreateAsync(
        CreateGenreRequest request,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync<Genre>(
            Resource,
            request,
            cancellationToken);
    }

    public Task UpdateAsync(
        Guid id,
        UpdateGenreRequest request,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PutAsync(
            $"{Resource}/{id}",
            request,
            cancellationToken);
    }

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.DeleteAsync(
            $"{Resource}/{id}",
            cancellationToken);
    }
}
