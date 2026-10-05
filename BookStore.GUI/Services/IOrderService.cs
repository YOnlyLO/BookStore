using BookStore.GUI.Models;

namespace BookStore.GUI.Services;

public interface IOrderService
{
    Task<IReadOnlyList<Order>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<Order> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Order> CreateAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken = default);

    Task AddItemAsync(
        Guid orderId,
        AddOrderItemRequest request,
        CancellationToken cancellationToken = default);

    Task RemoveItemAsync(
        Guid orderId,
        Guid bookId,
        CancellationToken cancellationToken = default);

    Task ConfirmAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task CompleteAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task CancelAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}
