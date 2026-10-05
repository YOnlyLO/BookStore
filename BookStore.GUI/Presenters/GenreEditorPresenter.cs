using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

public sealed class GenreEditorPresenter : EditorPresenter<IGenreEditorView>
{
    private readonly IGenreService _genreService;
    private readonly IReadOnlyList<Genre> _existingGenres;
    private readonly Genre? _genre;

    /// <param name="genre">Изменяемый жанр; null — создание нового.</param>
    public GenreEditorPresenter(
        IGenreEditorView view,
        IGenreService genreService,
        IReadOnlyList<Genre> existingGenres,
        Genre? genre)
        : base(view)
    {
        _genreService = genreService;
        _existingGenres = existingGenres;
        _genre = genre;

        View.Title = genre is null ? "Новый жанр" : "Изменение жанра";
        View.GenreName = genre?.Name ?? string.Empty;
    }

    private string Name => View.GenreName.Trim();

    protected override ValidationError? Validate()
    {
        if (CheckText(nameof(View.GenreName), Name, "Название", 200) is { } error)
            return error;

        // В БД на названии жанра уникальный индекс: без этой проверки API ответит ошибкой 500.
        var isDuplicate = _existingGenres.Any(genre =>
            genre.Id != _genre?.Id &&
            string.Equals(genre.Name, Name, StringComparison.CurrentCultureIgnoreCase));

        return isDuplicate
            ? new ValidationError(nameof(View.GenreName), "Жанр с таким названием уже существует.")
            : null;
    }

    protected override async Task<Guid> SaveAsync()
    {
        if (_genre is null)
        {
            var created = await _genreService.CreateAsync(new CreateGenreRequest(Name));

            return created.Id;
        }

        await _genreService.UpdateAsync(_genre.Id, new UpdateGenreRequest(Name));

        return _genre.Id;
    }
}
