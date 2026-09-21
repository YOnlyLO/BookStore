using BookStore.API.DTOs.Books;
using BookStore.Core.Models;

namespace BookStore.API.Mappings;

public static class BookMapping
{
    public static BookResponse ToResponse(this Book book)
    {
        return new BookResponse(
            book.Id,
            book.Title,
            book.Author,
            book.Description,
            book.Price,
            book.StockQuantity,
            book.GenreId);
    }
}
