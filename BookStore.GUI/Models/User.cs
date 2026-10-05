namespace BookStore.GUI.Models;

public sealed record User(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);

public sealed record CreateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string PasswordHash);

public sealed record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Email);
