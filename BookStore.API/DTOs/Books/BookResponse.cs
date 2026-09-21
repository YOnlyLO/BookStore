namespace BookStore.API.DTOs.Books;

public record BookResponse(
    Guid Id,
    string Title,
    string Author,
    string? Description,
    decimal Price,
    int StockQuantity,
    Guid GenreId);
    