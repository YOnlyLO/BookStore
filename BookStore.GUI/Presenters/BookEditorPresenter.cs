using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

public sealed class BookEditorPresenter : EditorPresenter<IBookEditorView>
{
    private readonly IBookService _bookService;
    private readonly Book? _book;

    /// <param name="book">Изменяемая книга; null — создание новой.</param>
    public BookEditorPresenter(
        IBookEditorView view,
        IBookService bookService,
        IReadOnlyList<Genre> genres,
        Book? book)
        : base(view)
    {
        _bookService = bookService;
        _book = book;

        View.Title = book is null ? "Новая книга" : "Изменение книги";

        View.SetGenres(genres
            .OrderBy(genre => genre.Name, StringComparer.CurrentCulture)
            .Select(genre => new LookupItem(genre.Id, genre.Name))
            .ToList());

        View.BookTitle = book?.Title ?? string.Empty;
        View.Author = book?.Author ?? string.Empty;
        View.Description = book?.Description ?? string.Empty;
        View.Price = book?.Price ?? 0;
        View.StockQuantity = book?.StockQuantity ?? 0;
        View.GenreId = book?.GenreId;
        View.IsStockQuantityEditable = book is null;
    }

    private string Title => View.BookTitle.Trim();

    private string Author => View.Author.Trim();

    private string? Description =>
        string.IsNullOrWhiteSpace(View.Description) ? null : View.Description.Trim();

    protected override ValidationError? Validate()
    {
        return CheckText(nameof(View.BookTitle), Title, "Название", 200)
            ?? CheckText(nameof(View.Author), Author, "Автор", 200)
            ?? CheckText(nameof(View.Description), Description ?? string.Empty, "Описание", 2000, isRequired: false)
            ?? CheckPrice()
            ?? CheckStockQuantity()
            ?? CheckGenre();
    }

    private ValidationError? CheckPrice() =>
        View.Price <= 0
            ? new ValidationError(nameof(View.Price), "Цена книги должна быть больше нуля.")
            : null;

    private ValidationError? CheckStockQuantity() =>
        View.StockQuantity < 0
            ? new ValidationError(nameof(View.StockQuantity), "Остаток на складе не может быть отрицательным.")
            : null;

    private ValidationError? CheckGenre() =>
        View.GenreId is null
            ? new ValidationError(nameof(View.GenreId), "Выберите жанр книги.")
            : null;

    protected override async Task<Guid> SaveAsync()
    {
        var genreId = View.GenreId!.Value;

        if (_book is null)
        {
            var created = await _bookService.CreateAsync(new CreateBookRequest(
                Title, Author, Description, View.Price, View.StockQuantity, genreId));

            return created.Id;
        }

        await _bookService.UpdateAsync(_book.Id, new UpdateBookRequest(
            Title, Author, Description, View.Price, genreId));

        return _book.Id;
    }
}
