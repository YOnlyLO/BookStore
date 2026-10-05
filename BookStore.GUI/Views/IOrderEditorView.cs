using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Views;

/// <summary>
/// Окно заказа: состав (меняется только в статусе «Новый») и переходы между статусами.
/// </summary>
public interface IOrderEditorView : IDisposable
{
    /// <summary>Окно показано пользователю — можно загружать заказ.</summary>
    event EventHandler Started;

    event EventHandler BookChanged;

    event EventHandler AddItemRequested;

    event EventHandler RemoveItemRequested;

    event EventHandler ConfirmRequested;

    event EventHandler CompleteRequested;

    event EventHandler CancelRequested;

    Guid? BookId { get; set; }

    int Quantity { get; set; }

    decimal UnitPrice { get; set; }

    /// <summary>Книга выбранной позиции заказа.</summary>
    Guid? SelectedItemBookId { get; }

    void ShowModal();

    void SetBooks(IReadOnlyList<LookupItem> books);

    void ShowOrder(OrderDetails order, OrderActions actions);

    void SetBusy(bool isBusy);

    void ShowError(string? message);

    void ShowFieldError(string field, string message);

    bool Confirm(string title, string message, string confirmText);
}
