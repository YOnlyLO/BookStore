using BookStore.GUI.Models;
using BookStore.GUI.ViewModels;

namespace BookStore.GUI.Presenters;

/// <summary>Общие правила отображения заказов и пользователей для дашборда и раздела «Заказы».</summary>
internal static class OrderPresentation
{
    public static string Number(Guid orderId) =>
        orderId.ToString()[..8].ToUpperInvariant();

    public static Badge StatusBadge(OrderStatus status) => status switch
    {
        OrderStatus.New => new Badge("Новый", Tone.Info),
        OrderStatus.Confirmed => new Badge("Подтверждён", Tone.Accent),
        OrderStatus.Completed => new Badge("Завершён", Tone.Success),
        OrderStatus.Cancelled => new Badge("Отменён", Tone.Neutral),
        _ => new Badge(status.ToString(), Tone.Neutral)
    };

    public static string UserName(User user) =>
        $"{user.LastName} {user.FirstName}";

    public static IReadOnlyDictionary<Guid, string> UserNames(IEnumerable<User> users) =>
        users.ToDictionary(user => user.Id, UserName);

    public static OrderRow ToRow(
        Order order,
        IReadOnlyDictionary<Guid, string> userNames)
    {
        return new OrderRow(
            order.Id,
            Number(order.Id),
            userNames.GetValueOrDefault(order.UserId, "Неизвестный пользователь"),
            order.CreatedAt.ToLocalTime(),
            StatusBadge(order.Status),
            order.Items.Sum(item => item.Quantity),
            order.TotalPrice);
    }
}
