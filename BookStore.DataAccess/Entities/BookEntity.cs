namespace BookStore.DataAccess.Entities;

public class BookEntity
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public Guid GenreId { get; set; }

    public GenreEntity Genre { get; set; } = null!;

    public ICollection<OrderItemEntity> OrderItems { get; set; }
        = new List<OrderItemEntity>();
}