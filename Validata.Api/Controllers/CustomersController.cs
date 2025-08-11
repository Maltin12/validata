using MediatR;
using Microsoft.AspNetCore.Mvc;
using Validata.Application.Common.DTOs;
using Validata.Application.Customers.Commands;
using Validata.Application.Customers.Queries;

namespace Validata.Api.Controllers;

/// <summary>
/// API controller for managing customers.
/// </summary>
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomersController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator used to send commands and queries.</param>
    public CustomersController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="cmd">The command containing customer creation details.</param>
    /// <returns>The identifier of the newly created customer.</returns>
    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCustomerCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(GetById), new { id }, id);
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="id">The customer identifier from the route.</param>
    /// <param name="cmd">The command containing updated customer details.</param>
    /// <returns>No content if successful, otherwise an error response.</returns>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] UpdateCustomerCommand cmd)
    {
        if (id != cmd.Id) return BadRequest("Id mismatch.");
        var ok = await _mediator.Send(cmd);
        return ok ? NoContent() : NotFound();
    }

    /// <summary>
    /// Deletes a customer by identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete.</param>
    /// <returns>No content if successful, otherwise an error response.</returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var ok = await _mediator.Send(new DeleteCustomerCommand(id));
        return ok ? NoContent() : NotFound();
    }

    /// <summary>
    /// Retrieves a customer by identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to retrieve.</param>
    /// <returns>The customer details if found, otherwise not found.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerDto>> GetById(Guid id)
    {
        var dto = await _mediator.Send(new GetCustomerByIdQuery(id));
        return dto is null ? NotFound() : Ok(dto);
    }

    /// <summary>
    /// Retrieves all orders for a specific customer, sorted by date.
    /// </summary>
    /// <param name="id">The unique identifier of the customer whose orders to retrieve.</param>
    /// <returns>A list of orders for the specified customer.</returns>
    [HttpGet("{id:guid}/orders")]
    public async Task<ActionResult<List<OrderOutDto>>> GetOrders(Guid id)
        => Ok(await _mediator.Send(new GetCustomerOrdersByDateQuery(id)));
}
