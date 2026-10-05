using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Views;

public interface IBookEditorView : IEditorView
{
    string BookTitle { get; set; }

    string Author { get; set; }

    string Description { get; set; }

    decimal Price { get; set; }

    int StockQuantity { get; set; }

    /// <summary>Остаток задаётся только при создании книги: API не меняет его при обновлении.</summary>
    bool IsStockQuantityEditable { set; }

    Guid? GenreId { get; set; }

    void SetGenres(IReadOnlyList<LookupItem> genres);
}
