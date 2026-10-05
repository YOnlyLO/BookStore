using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

public sealed class UsersPresenter : RecordListPresenter<User, UserRow>
{
    private static readonly GridColumn[] Columns =
    [
        new(nameof(UserRow.FullName), "Фамилия и имя", 240),
        new(nameof(UserRow.Email), "Email", 260),
        new(nameof(UserRow.OrderCount), "Заказов", 90, ColumnAlignment.Right)
    ];

    private readonly IViewFactory _viewFactory;
    private readonly IUserService _userService;
    private readonly IOrderService _orderService;

    private Dictionary<Guid, int> _orderCounts = [];

    public UsersPresenter(
        IRecordListView view,
        IViewFactory viewFactory,
        IUserService userService,
        IOrderService orderService)
        : base(view, "Пользователи", "Учётные записи покупателей", Columns)
    {
        _viewFactory = viewFactory;
        _userService = userService;
        _orderService = orderService;
    }

    protected override string EmptyText => "Пользователей пока нет. Нажмите «Добавить», чтобы зарегистрировать первого.";

    protected override string DeleteConflictHint => "Возможно, у пользователя есть заказы.";

    protected override async Task<IReadOnlyList<User>> LoadAsync()
    {
        var usersTask = _userService.GetAllAsync();
        var ordersTask = _orderService.GetAllAsync();

        await Task.WhenAll(usersTask, ordersTask);

        _orderCounts = ordersTask.Result
            .GroupBy(order => order.UserId)
            .ToDictionary(group => group.Key, group => group.Count());

        return usersTask.Result
            .OrderBy(OrderPresentation.UserName, StringComparer.CurrentCulture)
            .ToList();
    }

    protected override Guid GetId(User record) => record.Id;

    protected override UserRow ToRow(User record) =>
        new UserRow(
            record.Id,
            OrderPresentation.UserName(record),
            record.Email,
            _orderCounts.GetValueOrDefault(record.Id));

    protected override IEnumerable<string?> GetSearchableText(User record) =>
        [record.FirstName, record.LastName, record.Email];

    protected override Guid? OpenCreateEditor() => OpenEditor(null);

    protected override bool OpenEditEditor(User record) => OpenEditor(record) is not null;

    protected override string GetDeleteQuestion(User record) =>
        $"Удалить пользователя {OrderPresentation.UserName(record)} ({record.Email})?";

    // Заказы ссылаются на пользователя с ограничением Restrict: сервер не даст удалить такого пользователя.
    protected override Task<string?> GetDeleteBlockerAsync(User record)
    {
        var orderCount = _orderCounts.GetValueOrDefault(record.Id);

        return Task.FromResult(orderCount > 0
            ? $"У пользователя {OrderPresentation.UserName(record)} есть заказы ({orderCount}). Сначала удалите их."
            : null);
    }

    protected override Task DeleteAsync(Guid id) => _userService.DeleteAsync(id);

    private Guid? OpenEditor(User? user)
    {
        using var view = _viewFactory.CreateUserEditor();

        return new UserEditorPresenter(view, _userService, Records, user).Run();
    }
}
