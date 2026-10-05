using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

/// <summary>
/// Изменение заказа. Отдельного PUT для заказа в API нет: каждое действие
/// (добавить или убрать позицию, сменить статус) сразу отправляется на сервер,
/// после чего заказ перечитывается.
/// </summary>
public sealed class OrderEditorPresenter
{
    private readonly IOrderEditorView _view;
    private readonly IOrderService _orderService;
    private readonly Guid _orderId;
    private readonly IReadOnlyDictionary<Guid, Book> _books;
    private readonly IReadOnlyDictionary<Guid, string> _userNames;

    private Order? _order;
    private bool _isBusy;

    public OrderEditorPresenter(
        IOrderEditorView view,
        IOrderService orderService,
        Guid orderId,
        IReadOnlyList<Book> books,
        IReadOnlyDictionary<Guid, string> userNames)
    {
        _view = view;
        _orderService = orderService;
        _orderId = orderId;
        _books = books.ToDictionary(book => book.Id);
        _userNames = userNames;

        _view.SetBooks(books
            .OrderBy(book => book.Title, StringComparer.CurrentCulture)
            .Select(book => new LookupItem(
                book.Id,
                $"{book.Title} — {book.Author} · {book.Price:C2} · на складе: {book.StockQuantity}"))
            .ToList());

        ResetItemForm();

        _view.Started += async (_, _) => await ExecuteAsync(() => Task.CompletedTask);
        _view.BookChanged += (_, _) => OnBookChanged();
        _view.AddItemRequested += async (_, _) => await AddItemAsync();
        _view.RemoveItemRequested += async (_, _) => await RemoveItemAsync();
        _view.ConfirmRequested += async (_, _) => await ExecuteAsync(() => _orderService.ConfirmAsync(_orderId));
        _view.CompleteRequested += async (_, _) => await ExecuteAsync(() => _orderService.CompleteAsync(_orderId));
        _view.CancelRequested += async (_, _) => await CancelAsync();
    }

    public void Run() => _view.ShowModal();

    private void OnBookChanged()
    {
        // Цену позиции подставляем из карточки книги — её можно поправить вручную.
        if (_view.BookId is { } bookId && _books.TryGetValue(bookId, out var book))
            _view.UnitPrice = book.Price;
    }

    private async Task AddItemAsync()
    {
        if (_view.BookId is not { } bookId)
        {
            _view.ShowFieldError(nameof(_view.BookId), "Выберите книгу.");
            return;
        }

        if (_view.Quantity <= 0)
        {
            _view.ShowFieldError(nameof(_view.Quantity), "Количество должно быть больше нуля.");
            return;
        }

        if (_view.UnitPrice <= 0)
        {
            _view.ShowFieldError(nameof(_view.UnitPrice), "Цена должна быть больше нуля.");
            return;
        }

        var request = new AddOrderItemRequest(bookId, _view.Quantity, _view.UnitPrice);

        await ExecuteAsync(
            () => _orderService.AddItemAsync(_orderId, request),
            ResetItemForm);
    }

    private async Task RemoveItemAsync()
    {
        if (_view.SelectedItemBookId is not { } bookId)
            return;

        await ExecuteAsync(() => _orderService.RemoveItemAsync(_orderId, bookId));
    }

    private async Task CancelAsync()
    {
        var question =
            $"Отменить заказ #{OrderPresentation.Number(_orderId)}? " +
            "Отменённый заказ нельзя будет изменить или снова подтвердить.";

        if (!_view.Confirm("Отмена заказа", question, "Отменить заказ"))
            return;

        await ExecuteAsync(() => _orderService.CancelAsync(_orderId));
    }

    /// <summary>Выполняет действие над заказом и перечитывает его с сервера.</summary>
    private async Task ExecuteAsync(Func<Task> action, Action? onSuccess = null)
    {
        if (_isBusy)
            return;

        _isBusy = true;
        _view.SetBusy(true);
        _view.ShowError(null);

        try
        {
            await action();
            onSuccess?.Invoke();
        }
        catch (ApiException exception)
        {
            _view.ShowError(exception.Message);
        }

        try
        {
            _order = await _orderService.GetByIdAsync(_orderId);
            Render(_order);
        }
        catch (ApiException exception)
        {
            _view.ShowError(exception.Message);
        }
        finally
        {
            _isBusy = false;
            _view.SetBusy(false);
        }
    }

    private void Render(Order order)
    {
        var items = order.Items
            .Select(item => new OrderItemRow(
                item.Id,
                item.BookId,
                _books.TryGetValue(item.BookId, out var book) ? book.Title : "Неизвестная книга",
                item.Quantity,
                item.UnitPrice,
                item.TotalPrice))
            .ToList();

        var details = new OrderDetails(
            OrderPresentation.Number(order.Id),
            _userNames.GetValueOrDefault(order.UserId, "Неизвестный пользователь"),
            order.CreatedAt.ToLocalTime(),
            OrderPresentation.StatusBadge(order.Status),
            items,
            order.TotalPrice);

        var isNew = order.Status == OrderStatus.New;
        var isConfirmed = order.Status == OrderStatus.Confirmed;

        var actions = new OrderActions(
            CanEditItems: isNew,
            CanConfirm: isNew && order.Items.Count > 0,
            CanComplete: isConfirmed,
            CanCancel: isNew || isConfirmed);

        _view.ShowOrder(details, actions);
    }

    private void ResetItemForm()
    {
        _view.BookId = null;
        _view.Quantity = 1;
        _view.UnitPrice = 0;
    }
}
