using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

public sealed class OrdersPresenter : RecordListPresenter<Order, OrderRow>
{
    private static readonly GridColumn[] Columns =
    [
        new(nameof(OrderRow.Number), "Номер", 90),
        new(nameof(OrderRow.Customer), "Покупатель", 220),
        new(nameof(OrderRow.CreatedAt), "Создан", 140, Format: "dd.MM.yyyy HH:mm"),
        new(nameof(OrderRow.Status), "Статус", 130),
        new(nameof(OrderRow.ItemCount), "Книг", 70, ColumnAlignment.Right),
        new(nameof(OrderRow.TotalPrice), "Сумма", 110, ColumnAlignment.Right, "C2")
    ];

    private readonly IViewFactory _viewFactory;
    private readonly IOrderService _orderService;
    private readonly IUserService _userService;
    private readonly IBookService _bookService;

    private IReadOnlyList<User> _users = [];
    private IReadOnlyList<Book> _books = [];
    private IReadOnlyDictionary<Guid, string> _userNames = new Dictionary<Guid, string>();

    public OrdersPresenter(
        IRecordListView view,
        IViewFactory viewFactory,
        IOrderService orderService,
        IUserService userService,
        IBookService bookService)
        : base(view, "Заказы", "Состав заказов и их статусы", Columns)
    {
        _viewFactory = viewFactory;
        _orderService = orderService;
        _userService = userService;
        _bookService = bookService;
    }

    protected override string EmptyText => "Заказов пока нет. Нажмите «Добавить», чтобы оформить первый.";

    protected override async Task<IReadOnlyList<Order>> LoadAsync()
    {
        var ordersTask = _orderService.GetAllAsync();
        var usersTask = _userService.GetAllAsync();
        var booksTask = _bookService.GetAllAsync();

        await Task.WhenAll(ordersTask, usersTask, booksTask);

        _users = usersTask.Result;
        _books = booksTask.Result;
        _userNames = OrderPresentation.UserNames(_users);

        return ordersTask.Result
            .OrderByDescending(order => order.CreatedAt)
            .ToList();
    }

    protected override Guid GetId(Order record) => record.Id;

    protected override OrderRow ToRow(Order record) =>
        OrderPresentation.ToRow(record, _userNames);

    protected override IEnumerable<string?> GetSearchableText(Order record) =>
    [
        OrderPresentation.Number(record.Id),
        _userNames.GetValueOrDefault(record.UserId),
        OrderPresentation.StatusBadge(record.Status).Text
    ];

    protected override Guid? OpenCreateEditor()
    {
        if (_users.Count == 0)
        {
            View.ShowAlert("Нет покупателей", "Заказ оформляется на пользователя. Сначала добавьте хотя бы одного пользователя.");
            return null;
        }

        Guid? orderId;

        using (var view = _viewFactory.CreateOrderCreator())
        {
            orderId = new OrderCreatePresenter(view, _orderService, _users).Run();
        }

        // Заказ создаётся пустым — сразу открываем его, чтобы добавить книги.
        if (orderId is { } id)
            OpenOrderEditor(id);

        return orderId;
    }

    protected override bool OpenEditEditor(Order record)
    {
        OpenOrderEditor(record.Id);

        // Изменения применяются сразу при каждом действии в окне заказа.
        return true;
    }

    protected override string GetDeleteQuestion(Order record) =>
        $"Удалить заказ #{OrderPresentation.Number(record.Id)} " +
        $"({_userNames.GetValueOrDefault(record.UserId, "неизвестный пользователь")})? " +
        "Позиции заказа будут удалены вместе с ним.";

    protected override Task DeleteAsync(Guid id) => _orderService.DeleteAsync(id);

    private void OpenOrderEditor(Guid orderId)
    {
        using var view = _viewFactory.CreateOrderEditor();

        new OrderEditorPresenter(view, _orderService, orderId, _books, _userNames).Run();
    }
}
