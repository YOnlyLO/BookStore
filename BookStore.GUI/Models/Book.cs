namespace BookStore.GUI.Models;

public sealed record Book(
    Guid Id,
    string Title,
    string Author,
    string? Description,
    decimal Price,
    int StockQuantity,
    Guid GenreId);

public sealed record CreateBookRequest(
    string Title,
    string Author,
    string? Description,
    decimal Price,
    int StockQuantity,
    Guid GenreId);

// API не позволяет менять остаток на складе при обновлении книги.
public sealed record UpdateBookRequest(
    string Title,
    string Author,
    string? Description,
    decimal Price,
    Guid GenreId);
