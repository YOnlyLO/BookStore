namespace BookStore.GUI.ViewModels;

/// <summary>Данные заказа для окна редактирования.</summary>
public sealed record OrderDetails(
    string Number,
    string Customer,
    DateTime CreatedAt,
    Badge Status,
    IReadOnlyList<OrderItemRow> Items,
    decimal TotalPrice);

/// <summary>Какие действия над заказом доступны в его текущем статусе.</summary>
public sealed record OrderActions(
    bool CanEditItems,
    bool CanConfirm,
    bool CanComplete,
    bool CanCancel);
