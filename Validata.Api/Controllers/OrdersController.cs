using MediatR;
using Microsoft.AspNetCore.Mvc;
using Validata.Application.Common.DTOs;
using Validata.Application.Orders.Commands;
using Validata.Application.Orders.Queries;

namespace Validata.Api.Controllers;

/// <summary>
/// API controller for managing orders.
/// </summary>
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="OrdersController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used to send commands and queries.</param>
    public OrdersController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Creates a new order.
    /// </summary>
    /// <param name="cmd">The command containing order creation details.</param>
    /// <returns>The identifier of the newly created order.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateOrderCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Retrieves an order by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order.</param>
    /// <returns>The order details if found, otherwise not found.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderOutDto>> GetById(Guid id)
    {
        var dto = await _mediator.Send(new GetOrderByIdQuery(id));
        return dto is null ? NotFound() : Ok(dto);
    }

    /// <summary>
    /// Updates an existing order.
    /// </summary>
    /// <param name="id">The order identifier from the route.</param>
    /// <param name="cmd">The command containing updated order details.</param>
    /// <returns>No content if successful, otherwise an error response.</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateOrderCommand cmd)
    {
        if (id != cmd.OrderId) return BadRequest("Id mismatch.");
        var ok = await _mediator.Send(cmd);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes an order by identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the order to delete.</param>
    /// <returns>No content if successful, otherwise an error response.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var ok = await _mediator.Send(new DeleteOrderCommand(id));
        return ok ? NoContent() : NotFound();
    }
}
