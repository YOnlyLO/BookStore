using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

public sealed class BooksPresenter : RecordListPresenter<Book, BookRow>
{
    private static readonly GridColumn[] Columns =
    [
        new(nameof(BookRow.Title), "Название", 260),
        new(nameof(BookRow.Author), "Автор", 180),
        new(nameof(BookRow.Genre), "Жанр", 140),
        new(nameof(BookRow.Price), "Цена", 100, ColumnAlignment.Right, "C2"),
        new(nameof(BookRow.StockQuantity), "На складе", 90, ColumnAlignment.Right)
    ];

    private readonly IViewFactory _viewFactory;
    private readonly IBookService _bookService;
    private readonly IGenreService _genreService;
    private readonly IOrderService _orderService;

    private IReadOnlyList<Genre> _genres = [];
    private Dictionary<Guid, string> _genreNames = [];

    public BooksPresenter(
        IRecordListView view,
        IViewFactory viewFactory,
        IBookService bookService,
        IGenreService genreService,
        IOrderService orderService)
        : base(view, "Книги", "Каталог: авторы, цены, остатки на складе", Columns)
    {
        _viewFactory = viewFactory;
        _bookService = bookService;
        _genreService = genreService;
        _orderService = orderService;
    }

    protected override string EmptyText => "Книг пока нет. Нажмите «Добавить», чтобы внести первую.";

    protected override string DeleteConflictHint => "Возможно, книга входит в заказы.";

    protected override async Task<IReadOnlyList<Book>> LoadAsync()
    {
        var booksTask = _bookService.GetAllAsync();
        var genresTask = _genreService.GetAllAsync();

        await Task.WhenAll(booksTask, genresTask);

        _genres = genresTask.Result;
        _genreNames = _genres.ToDictionary(genre => genre.Id, genre => genre.Name);

        return booksTask.Result
            .OrderBy(book => book.Title, StringComparer.CurrentCulture)
            .ToList();
    }

    protected override Guid GetId(Book record) => record.Id;

    protected override BookRow ToRow(Book record) =>
        new BookRow(
            record.Id,
            record.Title,
            record.Author,
            GenreName(record),
            record.Price,
            record.StockQuantity);

    protected override IEnumerable<string?> GetSearchableText(Book record) =>
        [record.Title, record.Author, GenreName(record)];

    protected override Guid? OpenCreateEditor()
    {
        if (_genres.Count == 0)
        {
            View.ShowAlert("Нет жанров", "Книга должна относиться к жанру. Сначала создайте хотя бы один жанр.");
            return null;
        }

        return OpenEditor(null);
    }

    protected override bool OpenEditEditor(Book record) => OpenEditor(record) is not null;

    protected override string GetDeleteQuestion(Book record) =>
        $"Удалить книгу «{record.Title}» ({record.Author})?";

    // Позиции заказов ссылаются на книгу с ограничением Restrict: сервер не даст удалить такую книгу.
    protected override async Task<string?> GetDeleteBlockerAsync(Book record)
    {
        var orders = await _orderService.GetAllAsync();

        var orderCount = orders.Count(order =>
            order.Items.Any(item => item.BookId == record.Id));

        return orderCount > 0
            ? $"Книга «{record.Title}» входит в заказы ({orderCount}). Сначала уберите её из заказов или удалите эти заказы."
            : null;
    }

    protected override Task DeleteAsync(Guid id) => _bookService.DeleteAsync(id);

    private string GenreName(Book book) =>
        _genreNames.GetValueOrDefault(book.GenreId, "—");

    private Guid? OpenEditor(Book? book)
    {
        using var view = _viewFactory.CreateBookEditor();

        return new BookEditorPresenter(view, _bookService, _genres, book).Run();
    }
}
