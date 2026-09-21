using BookStore.Core.Enums;

namespace BookStore.API.DTOs.Orders;

public record OrderResponse(
    Guid Id,
    Guid UserId,
    DateTime CreatedAt,
    OrderStatus Status,
    IReadOnlyCollection<OrderItemResponse> Items,
    decimal TotalPrice);
    