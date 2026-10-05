namespace BookStore.GUI.Models;

// Значения совпадают с BookStore.Core.Enums.OrderStatus: API сериализует enum числом.
public enum OrderStatus
{
    New = 0,
    Confirmed = 1,
    Completed = 2,
    Cancelled = 3
}

public sealed record OrderItem(
    Guid Id,
    Guid BookId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);

public sealed record Order(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    OrderStatus Status,
    IReadOnlyList<OrderItem> Items,
    decimal TotalPrice);

public sealed record CreateOrderRequest(
    Guid UserId);

public sealed record AddOrderItemRequest(
    Guid BookId,
    int Quantity,
    decimal UnitPrice);
