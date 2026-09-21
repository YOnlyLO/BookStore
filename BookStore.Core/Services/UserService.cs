using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Services;

public class UserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _userRepository.GetByIdAsync(
            id,
            cancellationToken);
    }

    public Task<IReadOnlyList<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _userRepository.GetAllAsync(
            cancellationToken);
    }

    public async Task<Result<User>> CreateAsync(
        string firstName,
        string lastName,
        string email,
        string passwordHash,
        CancellationToken cancellationToken = default)
    {
        var result = User.Create(
            firstName,
            lastName,
            email,
            passwordHash);

        if (result.IsFailure)
            return result;

        await _userRepository.AddAsync(
            result.Value,
            cancellationToken);

        return result;
    }

    public async Task<Result> UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByIdAsync(
            user.Id,
            cancellationToken);

        if (existingUser is null)
            return Result.Failure("User not found.");

        await _userRepository.UpdateAsync(
            user,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (user is null)
            return Result.Failure("User not found.");

        await _userRepository.DeleteAsync(
            id,
            cancellationToken);

        return Result.Success();
    }
}
