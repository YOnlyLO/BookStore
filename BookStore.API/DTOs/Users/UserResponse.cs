namespace BookStore.API.DTOs.Users;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email);
    