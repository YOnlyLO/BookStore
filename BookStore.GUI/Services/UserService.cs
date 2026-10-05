using BookStore.GUI.Models;

namespace BookStore.GUI.Services;

public sealed class UserService : IUserService
{
    private const string Resource = "api/users";

    private readonly ApiClient _apiClient;

    public UserService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _apiClient.GetAsync<IReadOnlyList<User>>(
            Resource,
            cancellationToken);
    }

    public Task<User> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync<User>(
            Resource,
            request,
            cancellationToken);
    }

    public Task UpdateAsync(
        Guid id,
        UpdateUserRequest request,
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
