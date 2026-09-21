namespace BookStore.API.DTOs.Orders;

public record AddOrderItemRequest(
    Guid BookId,
    int Quantity,
    decimal UnitPrice);
    