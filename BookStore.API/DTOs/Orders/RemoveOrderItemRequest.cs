namespace BookStore.API.DTOs.Orders;

public record RemoveOrderItemRequest(
    Guid BookId);