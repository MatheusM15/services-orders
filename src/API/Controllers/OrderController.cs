using API.Requets.Order;
using APPLICATION.Order.GetByIdAsync;
using APPLICATION.Order.GetOrdersGroupByStatus;
using APPLICATION.Order.NextStepOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/v1/[Controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<OrderController> _logger;

    public OrderController(IMediator mediator, ILogger<OrderController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync([FromQuery] GetOrderRequest request)
    {
        var result = await _mediator.Send(request.ToQuery());
        return Ok(result);
    }

    [HttpGet("{orderId}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(orderId));
        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }


    [HttpPost]
    public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderRequest order)
    {
        try
        {
            var result = await _mediator.Send(order.ToCommand());

            if (result == null) return BadRequest("Error to create Order");

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating order: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("GetOrdersGroupByStatus")]
    public async Task<IActionResult> GetOrdersGroupByStatusAsync()
    {
        var result = await _mediator.Send(new GetOrdersGroupByStatusQuery());
        return Ok(result);
    }

    [HttpPost]
    [Route(":nextstep/{orderId}")]
    public IActionResult NextStep([FromRoute] Guid orderId)
    {
        try
        {
            var result = _mediator.Send(new NextStepOrderCommand(orderId));
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating order: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
