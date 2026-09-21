using BookStore.API.DTOs.Orders;
using BookStore.API.Mappings;
using BookStore.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(cancellationToken);

        return Ok(orders.Select(order => order.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> GetById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(
            id,
            cancellationToken);

        if (order is null)
            return NotFound();

        return Ok(order.ToResponse());
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.CreateAsync(
            request.UserId,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        var response = result.Value.ToResponse();

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpPost("{orderId:guid}/items")]
    public async Task<IActionResult> AddItem(
        Guid orderId,
        AddOrderItemRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.AddItemAsync(
            orderId,
            request.BookId,
            request.Quantity,
            request.UnitPrice,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{orderId:guid}/items/{bookId:guid}")]
    public async Task<IActionResult> RemoveItem(
        Guid orderId,
        Guid bookId,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.RemoveItemAsync(
            orderId,
            bookId,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpPost("{orderId:guid}/confirm")]
    public async Task<IActionResult> Confirm(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.ConfirmAsync(
            orderId,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpPost("{orderId:guid}/complete")]
    public async Task<IActionResult> Complete(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.CompleteAsync(
            orderId,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpPost("{orderId:guid}/cancel")]
    public async Task<IActionResult> Cancel(
        Guid orderId,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.CancelAsync(
            orderId,
            cancellationToken);

        if (result.IsFailure)
            return BadRequest(result.Error);

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _orderService.DeleteAsync(
            id,
            cancellationToken);

        if (result.IsFailure)
            return NotFound(result.Error);

        return NoContent();
    }
}
