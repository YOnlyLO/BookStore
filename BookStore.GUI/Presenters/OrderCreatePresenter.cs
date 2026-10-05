using BookStore.GUI.Models;
using BookStore.GUI.Services;
using BookStore.GUI.ViewModels;
using BookStore.GUI.Views;

namespace BookStore.GUI.Presenters;

/// <summary>Создание заказа: API создаёт пустой заказ для выбранного пользователя.</summary>
public sealed class OrderCreatePresenter : EditorPresenter<IOrderCreateView>
{
    private readonly IOrderService _orderService;

    public OrderCreatePresenter(
        IOrderCreateView view,
        IOrderService orderService,
        IReadOnlyList<User> users)
        : base(view)
    {
        _orderService = orderService;

        View.Title = "Новый заказ";

        View.SetUsers(users
            .OrderBy(OrderPresentation.UserName, StringComparer.CurrentCulture)
            .Select(user => new LookupItem(user.Id, $"{OrderPresentation.UserName(user)} · {user.Email}"))
            .ToList());
    }

    protected override ValidationError? Validate() =>
        View.UserId is null
            ? new ValidationError(nameof(View.UserId), "Выберите покупателя.")
            : null;

    protected override async Task<Guid> SaveAsync()
    {
        var created = await _orderService.CreateAsync(new CreateOrderRequest(View.UserId!.Value));

        return created.Id;
    }
}
