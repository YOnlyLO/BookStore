using BookStore.GUI.Models;

namespace BookStore.GUI.Services;

public sealed class BookService : IBookService
{
    private const string Resource = "api/books";

    private readonly ApiClient _apiClient;

    public BookService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyList<Book>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _apiClient.GetAsync<IReadOnlyList<Book>>(
            Resource,
            cancellationToken);
    }

    public Task<Book> CreateAsync(
        CreateBookRequest request,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync<Book>(
            Resource,
            request,
            cancellationToken);
    }

    public Task UpdateAsync(
        Guid id,
        UpdateBookRequest request,
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
