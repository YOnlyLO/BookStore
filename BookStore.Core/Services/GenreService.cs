using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Services;

public class GenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public Task<Genre?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _genreRepository.GetByIdAsync(
            id,
            cancellationToken);
    }

    public Task<IReadOnlyList<Genre>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _genreRepository.GetAllAsync(
            cancellationToken);
    }

    public async Task<Result<Genre>> CreateAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var result = Genre.Create(name);

        if (result.IsFailure)
            return result;

        await _genreRepository.AddAsync(
            result.Value,
            cancellationToken);

        return result;
    }

    public async Task<Result> UpdateAsync(
        Genre genre,
        CancellationToken cancellationToken = default)
    {
        var existingGenre = await _genreRepository.GetByIdAsync(
            genre.Id,
            cancellationToken);

        if (existingGenre is null)
            return Result.Failure("Genre not found.");

        await _genreRepository.UpdateAsync(
            genre,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var genre = await _genreRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (genre is null)
            return Result.Failure("Genre not found.");

        await _genreRepository.DeleteAsync(
            id,
            cancellationToken);

        return Result.Success();
    }
}
