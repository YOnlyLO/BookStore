using BookStore.GUI.Models;

namespace BookStore.GUI.Services;

public interface IUserService
{
    Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<User> CreateAsync(
        CreateUserRequest request,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Guid id,
        UpdateUserRequest request,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
