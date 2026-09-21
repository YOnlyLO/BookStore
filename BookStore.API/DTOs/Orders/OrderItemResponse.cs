namespace BookStore.API.DTOs.Orders;

public record OrderItemResponse(
    Guid Id,
    Guid BookId,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice);
    