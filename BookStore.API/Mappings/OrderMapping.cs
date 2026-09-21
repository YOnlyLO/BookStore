using BookStore.API.DTOs.Orders;
using BookStore.Core.Models;

namespace BookStore.API.Mappings;

public static class OrderMapping
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse(
            order.Id,
            order.UserId,
            order.CreatedAt,
            order.Status,
            order.Items
                .Select(item => item.ToResponse())
                .ToList(),
            order.TotalPrice);
    }

    public static OrderItemResponse ToResponse(this OrderItem item)
    {
        return new OrderItemResponse(
            item.Id,
            item.BookId,
            item.Quantity,
            item.UnitPrice,
            item.TotalPrice);
    }
}