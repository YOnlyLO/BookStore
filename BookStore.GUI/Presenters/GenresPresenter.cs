using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

public sealed class GenresPresenter : RecordListPresenter<Genre, GenreRow>
{
    private static readonly GridColumn[] Columns =
    [
        new(nameof(GenreRow.Name), "Название", 300),
        new(nameof(GenreRow.BookCount), "Книг", 80, ColumnAlignment.Right)
    ];

    private readonly IViewFactory _viewFactory;
    private readonly IGenreService _genreService;
    private readonly IBookService _bookService;

    private Dictionary<Guid, int> _bookCounts = [];

    public GenresPresenter(
        IRecordListView view,
        IViewFactory viewFactory,
        IGenreService genreService,
        IBookService bookService)
        : base(view, "Жанры", "Справочник жанров, к которым относятся книги", Columns)
    {
        _viewFactory = viewFactory;
        _genreService = genreService;
        _bookService = bookService;
    }

    protected override string EmptyText => "Жанров пока нет. Нажмите «Добавить», чтобы создать первый.";

    protected override string DeleteConflictHint => "Возможно, к жанру относятся книги.";

    protected override async Task<IReadOnlyList<Genre>> LoadAsync()
    {
        var genresTask = _genreService.GetAllAsync();
        var booksTask = _bookService.GetAllAsync();

        await Task.WhenAll(genresTask, booksTask);

        _bookCounts = booksTask.Result
            .GroupBy(book => book.GenreId)
            .ToDictionary(group => group.Key, group => group.Count());

        return genresTask.Result
            .OrderBy(genre => genre.Name, StringComparer.CurrentCulture)
            .ToList();
    }

    protected override Guid GetId(Genre record) => record.Id;

    protected override GenreRow ToRow(Genre record) =>
        new GenreRow(record.Id, record.Name, _bookCounts.GetValueOrDefault(record.Id));

    protected override IEnumerable<string?> GetSearchableText(Genre record) => [record.Name];

    protected override Guid? OpenCreateEditor() => OpenEditor(null);

    protected override bool OpenEditEditor(Genre record) => OpenEditor(record) is not null;

    protected override string GetDeleteQuestion(Genre record) =>
        $"Удалить жанр «{record.Name}»?";

    // Книги ссылаются на жанр с ограничением Restrict: сервер не даст удалить такой жанр.
    protected override Task<string?> GetDeleteBlockerAsync(Genre record)
    {
        var bookCount = _bookCounts.GetValueOrDefault(record.Id);

        return Task.FromResult(bookCount > 0
            ? $"К жанру «{record.Name}» относятся книги ({bookCount}). Сначала удалите их или перенесите в другой жанр."
            : null);
    }

    protected override Task DeleteAsync(Guid id) => _genreService.DeleteAsync(id);

    private Guid? OpenEditor(Genre? genre)
    {
        using var view = _viewFactory.CreateGenreEditor();

        return new GenreEditorPresenter(view, _genreService, Records, genre).Run();
    }
}
