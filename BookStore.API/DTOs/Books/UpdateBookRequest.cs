namespace BookStore.API.DTOs.Books;

public record UpdateBookRequest(
    string Title,
    string Author,
    string? Description,
    decimal Price,
    Guid GenreId);
    