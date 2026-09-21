using BookStore.Core.Enums;

namespace BookStore.DataAccess.Entities;

public class OrderEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public OrderStatus Status { get; set; }

    public UserEntity User { get; set; } = null!;

    public ICollection<OrderItemEntity> Items { get; set; }
        = new List<OrderItemEntity>();
}