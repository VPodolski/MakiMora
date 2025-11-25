using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        [HttpGet]
        [Authorize(Roles = "manager,hr")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders([FromQuery] Guid? statusId, [FromQuery] Guid? locationId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            IEnumerable<OrderDto> orders;

            if (startDate.HasValue && endDate.HasValue)
            {
                orders = await _orderService.GetOrdersByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else if (statusId.HasValue && locationId.HasValue)
            {
                orders = await _orderService.GetOrdersByStatusAndLocationAsync(statusId.Value, locationId.Value);
            }
            else if (statusId.HasValue)
            {
                orders = await _orderService.GetOrdersByStatusAsync(statusId.Value);
            }
            else if (locationId.HasValue)
            {
                orders = await _orderService.GetOrdersByLocationAsync(locationId.Value);
            }
            else
            {
                orders = await _orderService.GetOrdersAsync();
            }

            return Ok(orders);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderRequestDto createOrderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var order = await _orderService.CreateOrderAsync(createOrderDto);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<OrderDto>> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var order = await _orderService.UpdateOrderStatusAsync(id, request.StatusId, request.Note);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("kitchen")]
        [Authorize(Roles = "sushi_chef")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetKitchenOrders([FromQuery] Guid locationId)
        {
            // Get orders with status "pending" or "preparing" for the kitchen
            var orders = await _orderService.GetOrdersByLocationAsync(locationId);
            var kitchenOrders = orders.Where(o => o.Status.Name == "pending" || o.Status.Name == "preparing");

            return Ok(kitchenOrders);
        }

        [HttpGet("packing")]
        [Authorize(Roles = "packer")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetPackingOrders([FromQuery] Guid locationId)
        {
            // Get orders with status "ready" for packing
            var orders = await _orderService.GetOrdersByLocationAsync(locationId);
            var packingOrders = orders.Where(o => o.Status.Name == "ready");

            return Ok(packingOrders);
        }

        [HttpPatch("{id}/items/{itemId}/status")]
        [Authorize(Roles = "sushi_chef")]
        public async Task<ActionResult<OrderDto>> UpdateOrderItemStatus(Guid id, Guid itemId, [FromBody] UpdateOrderItemStatusRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var order = await _orderService.UpdateOrderItemStatusAsync(itemId, request.StatusId, request.Note);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/packed")]
        [Authorize(Roles = "packer")]
        public async Task<ActionResult<OrderDto>> MarkOrderAsPacked(Guid id)
        {
            try
            {
                var order = await _orderService.MarkOrderAsAssembledAsync(id);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/assign-courier")]
        [Authorize(Roles = "courier")]
        public async Task<ActionResult<OrderDto>> AssignCourierToOrder(Guid id, [FromBody] AssignCourierRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var order = await _orderService.AssignCourierToOrderAsync(id, request.CourierId);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/delivered")]
        [Authorize(Roles = "courier")]
        public async Task<ActionResult<OrderDto>> MarkOrderAsDelivered(Guid id)
        {
            try
            {
                var order = await _orderService.MarkOrderAsDeliveredAsync(id);
                return Ok(order);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}