namespace BookStore.GUI.ViewModels;

/// <summary>Строка таблицы записей. Id не показывается, по нему представление сообщает выбранную запись.</summary>
public interface IRecordRow
{
    Guid Id { get; }
}

public sealed record GenreRow(
    Guid Id,
    string Name,
    int BookCount) : IRecordRow;

public sealed record BookRow(
    Guid Id,
    string Title,
    string Author,
    string Genre,
    decimal Price,
    int StockQuantity) : IRecordRow;

public sealed record UserRow(
    Guid Id,
    string FullName,
    string Email,
    int OrderCount) : IRecordRow;

public sealed record OrderRow(
    Guid Id,
    string Number,
    string Customer,
    DateTime CreatedAt,
    Badge Status,
    int ItemCount,
    decimal TotalPrice) : IRecordRow;

public sealed record OrderItemRow(
    Guid Id,
    Guid BookId,
    string Book,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice) : IRecordRow;
