namespace BookStore.DataAccess.Entities;

public class OrderItemEntity
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

    public Guid BookId { get; set; }

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public OrderEntity Order { get; set; } = null!;

    public BookEntity Book { get; set; } = null!;
}