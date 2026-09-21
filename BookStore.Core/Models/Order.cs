using BookStore.Core.Enums;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Models;

public class Order
{
    private readonly List<OrderItem> _items = [];

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public OrderStatus Status { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public decimal TotalPrice =>
        _items.Sum(item => item.TotalPrice);

    private Order(
        Guid userId,
        DateTime createdAt)
    {
        Id = Guid.NewGuid();

        UserId = userId;
        CreatedAt = createdAt;
        Status = OrderStatus.New;
    }

    public static Result<Order> Create(Guid userId)
    {
        if (userId == Guid.Empty)
            return Result.Failure<Order>(
                "Идентификатор пользователя не может быть пустым.");

        return Result.Success(
            new Order(
                userId,
                DateTime.UtcNow));
    }

    public Result AddItem(
        Guid bookId,
        int quantity,
        decimal unitPrice)
    {
        if (Status != OrderStatus.New)
            return Result.Failure(
                "Нельзя изменять заказ, который больше не находится в статусе нового.");

        if (bookId == Guid.Empty)
            return Result.Failure(
                "Идентификатор книги не может быть пустым.");

        if (quantity <= 0)
            return Result.Failure(
                "Количество товара должно быть больше нуля.");

        if (unitPrice <= 0)
            return Result.Failure(
                "Цена товара должна быть больше нуля.");

        var existingItem = _items.FirstOrDefault(
            item => item.BookId == bookId);

        if (existingItem is not null)
            return existingItem.IncreaseQuantity(quantity);

        var itemResult = OrderItem.Create(
            bookId,
            quantity,
            unitPrice);

        if (itemResult.IsFailure)
            return Result.Failure(itemResult.Error);

        _items.Add(itemResult.Value);

        return Result.Success();
    }

    public Result RemoveItem(Guid bookId)
    {
        if (Status != OrderStatus.New)
            return Result.Failure(
                "Нельзя изменять заказ, который больше не находится в статусе нового.");

        if (bookId == Guid.Empty)
            return Result.Failure(
                "Идентификатор книги не может быть пустым.");

        var item = _items.FirstOrDefault(
            item => item.BookId == bookId);

        if (item is null)
            return Result.Failure(
                "Указанная книга отсутствует в заказе.");

        _items.Remove(item);

        return Result.Success();
    }

    public Result Confirm()
    {
        if (Status != OrderStatus.New)
            return Result.Failure(
                "Подтвердить можно только новый заказ.");

        if (_items.Count == 0)
            return Result.Failure(
                "Нельзя подтвердить пустой заказ.");

        Status = OrderStatus.Confirmed;

        return Result.Success();
    }

    public Result Complete()
    {
        if (Status != OrderStatus.Confirmed)
            return Result.Failure(
                "Завершить можно только подтверждённый заказ.");

        Status = OrderStatus.Completed;

        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status is OrderStatus.Completed or OrderStatus.Cancelled)
            return Result.Failure(
                "Нельзя отменить завершённый или уже отменённый заказ.");

        Status = OrderStatus.Cancelled;

        return Result.Success();
    }
}
