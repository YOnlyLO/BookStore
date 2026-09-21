namespace BookStore.API.DTOs.Books;

public record CreateBookRequest(
    string Title,
    string Author,
    string? Description,
    decimal Price,
    int StockQuantity,
    Guid GenreId);
    