using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using CSharpFunctionalExtensions;

namespace BookStore.Core.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<Order?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return _orderRepository.GetByIdAsync(
            id,
            cancellationToken);
    }

    public Task<IReadOnlyList<Order>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return _orderRepository.GetAllAsync(
            cancellationToken);
    }

    public async Task<Result<Order>> CreateAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var result = Order.Create(userId);

        if (result.IsFailure)
            return result;

        await _orderRepository.AddAsync(
            result.Value,
            cancellationToken);

        return result;
    }

    public async Task<Result> AddItemAsync(
        Guid orderId,
        Guid bookId,
        int quantity,
        decimal unitPrice,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(
            orderId,
            cancellationToken);

        if (order is null)
            return Result.Failure("Order not found.");

        var result = order.AddItem(
            bookId,
            quantity,
            unitPrice);

        if (result.IsFailure)
            return result;

        await _orderRepository.UpdateAsync(
            order,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RemoveItemAsync(
        Guid orderId,
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(
            orderId,
            cancellationToken);

        if (order is null)
            return Result.Failure("Order not found.");

        var result = order.RemoveItem(bookId);

        if (result.IsFailure)
            return result;

        await _orderRepository.UpdateAsync(
            order,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> ConfirmAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(
            orderId,
            cancellationToken);

        if (order is null)
            return Result.Failure("Order not found.");

        var result = order.Confirm();

        if (result.IsFailure)
            return result;

        await _orderRepository.UpdateAsync(
            order,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> CompleteAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(
            orderId,
            cancellationToken);

        if (order is null)
            return Result.Failure("Order not found.");

        var result = order.Complete();

        if (result.IsFailure)
            return result;

        await _orderRepository.UpdateAsync(
            order,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> CancelAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(
            orderId,
            cancellationToken);

        if (order is null)
            return Result.Failure("Order not found.");

        var result = order.Cancel();

        if (result.IsFailure)
            return result;

        await _orderRepository.UpdateAsync(
            order,
            cancellationToken);

        return Result.Success();
    }

    public async Task<Result> DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (order is null)
            return Result.Failure("Order not found.");

        await _orderRepository.DeleteAsync(
            id,
            cancellationToken);

        return Result.Success();
    }
}
