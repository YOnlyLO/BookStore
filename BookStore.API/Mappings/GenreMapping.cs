using BookStore.API.DTOs.Genres;
using BookStore.Core.Models;

namespace BookStore.API.Mappings;

public static class GenreMapping
{
    public static GenreResponse ToResponse(this Genre genre)
    {
        return new GenreResponse(
            genre.Id,
            genre.Name);
    }
}