using BookStore.GUI.Models;

namespace BookStore.GUI.Services;

/// <summary>
/// Отдельного PUT для заказа в API нет: меняются состав (только в статусе «Новый»)
/// и статус — через переходы confirm/complete/cancel.
/// </summary>
public sealed class OrderService : IOrderService
{
    private const string Resource = "api/orders";

    private readonly ApiClient _apiClient;

    public OrderService(ApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public Task<IReadOnlyList<Order>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _apiClient.GetAsync<IReadOnlyList<Order>>(
            Resource,
            cancellationToken);
    }

    public Task<Order> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.GetAsync<Order>(
            $"{Resource}/{id}",
            cancellationToken);
    }

    public Task<Order> CreateAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync<Order>(
            Resource,
            request,
            cancellationToken);
    }

    public Task AddItemAsync(
        Guid orderId,
        AddOrderItemRequest request,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync(
            $"{Resource}/{orderId}/items",
            request,
            cancellationToken);
    }

    public Task RemoveItemAsync(
        Guid orderId,
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.DeleteAsync(
            $"{Resource}/{orderId}/items/{bookId}",
            cancellationToken);
    }

    public Task ConfirmAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync(
            $"{Resource}/{orderId}/confirm",
            cancellationToken: cancellationToken);
    }

    public Task CompleteAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync(
            $"{Resource}/{orderId}/complete",
            cancellationToken: cancellationToken);
    }

    public Task CancelAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.PostAsync(
            $"{Resource}/{orderId}/cancel",
            cancellationToken: cancellationToken);
    }

    public Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _apiClient.DeleteAsync(
            $"{Resource}/{id}",
            cancellationToken);
    }
}
