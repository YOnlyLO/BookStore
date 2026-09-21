using CSharpFunctionalExtensions;

namespace BookStore.Core.Models;

public class OrderItem
{
    public Guid Id { get; private set; }

    public Guid BookId { get; private set; }

    public int Quantity { get; private set; }

    public decimal UnitPrice { get; private set; }

    public decimal TotalPrice => Quantity * UnitPrice;

    private OrderItem(
        Guid bookId,
        int quantity,
        decimal unitPrice)
    {
        Id = Guid.NewGuid();

        BookId = bookId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }

    public static Result<OrderItem> Create(
        Guid bookId,
        int quantity,
        decimal unitPrice)
    {
        if (bookId == Guid.Empty)
            return Result.Failure<OrderItem>(
                "Идентификатор книги не может быть пустым.");

        if (quantity <= 0)
            return Result.Failure<OrderItem>(
                "Количество товара должно быть больше нуля.");

        if (unitPrice <= 0)
            return Result.Failure<OrderItem>(
                "Цена товара должна быть больше нуля.");

        return Result.Success(
            new OrderItem(
                bookId,
                quantity,
                unitPrice));
    }

    internal Result IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            return Result.Failure(
                "Количество для увеличения должно быть больше нуля.");

        Quantity += quantity;

        return Result.Success();
    }
}
