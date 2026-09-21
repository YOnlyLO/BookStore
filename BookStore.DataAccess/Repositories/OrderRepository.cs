using BookStore.Core.Interfaces;
using BookStore.Core.Models;
using BookStore.DataAccess.Context;
using BookStore.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStore.DataAccess.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly BookStoreDbContext _context;

    public OrderRepository(BookStoreDbContext context)
    {
        _context = context;
    }

    public async Task<Order?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Orders
            .AsNoTracking()
            .Include(order => order.Items)
            .FirstOrDefaultAsync(
                order => order.Id == id,
                cancellationToken);

        return entity is null
            ? null
            : MapToDomain(entity);
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var entities = await _context.Orders
            .AsNoTracking()
            .Include(order => order.Items)
            .ToListAsync(cancellationToken);

        return entities
            .Select(MapToDomain)
            .ToList();
    }

    public async Task AddAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        var entity = MapToEntity(order);

        await _context.Orders.AddAsync(
            entity,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Orders
            .Include(existingOrder => existingOrder.Items)
            .FirstOrDefaultAsync(
                existingOrder => existingOrder.Id == order.Id,
                cancellationToken);

        if (entity is null)
            return;

        entity.UserId = order.UserId;
        entity.CreatedAt = order.CreatedAt;
        entity.Status = order.Status;

        var domainItems = order.Items.ToList();

        var domainItemIds = domainItems
            .Select(item => item.Id)
            .ToHashSet();

        var itemsToDelete = entity.Items
            .Where(item => !domainItemIds.Contains(item.Id))
            .ToList();

        foreach (var item in itemsToDelete)
        {
            _context.OrderItems.Remove(item);
        }

        foreach (var domainItem in domainItems)
        {
            var existingItem = entity.Items
                .FirstOrDefault(item => item.Id == domainItem.Id);

            if (existingItem is null)
            {
                entity.Items.Add(new OrderItemEntity
                {
                    Id = domainItem.Id,
                    OrderId = entity.Id,
                    BookId = domainItem.BookId,
                    Quantity = domainItem.Quantity,
                    UnitPrice = domainItem.UnitPrice
                });

                continue;
            }

            existingItem.BookId = domainItem.BookId;
            existingItem.Quantity = domainItem.Quantity;
            existingItem.UnitPrice = domainItem.UnitPrice;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var entity = await _context.Orders
            .FirstOrDefaultAsync(
                order => order.Id == id,
                cancellationToken);

        if (entity is null)
            return;

        _context.Orders.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    private static Order MapToDomain(OrderEntity entity)
    {
        var items = entity.Items
            .Select(MapItemToDomain)
            .ToList();

        var result = Order.Restore(
            entity.Id,
            entity.UserId,
            entity.CreatedAt,
            entity.Status,
            items);

        return result.Value;
    }

    private static OrderItem MapItemToDomain(
        OrderItemEntity entity)
    {
        var result = OrderItem.Restore(
            entity.Id,
            entity.BookId,
            entity.Quantity,
            entity.UnitPrice);

        return result.Value;
    }

    private static OrderEntity MapToEntity(Order order)
    {
        var entity = new OrderEntity
        {
            Id = order.Id,
            UserId = order.UserId,
            CreatedAt = order.CreatedAt,
            Status = order.Status
        };

        foreach (var item in order.Items)
        {
            entity.Items.Add(new OrderItemEntity
            {
                Id = item.Id,
                OrderId = order.Id,
                BookId = item.BookId,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice
            });
        }

        return entity;
    }
}
