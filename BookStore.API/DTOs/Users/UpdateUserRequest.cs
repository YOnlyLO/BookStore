namespace BookStore.API.DTOs.Users;

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Email);
    