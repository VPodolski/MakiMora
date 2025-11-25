using Microsoft.AspNetCore.Mvc;
using MakiMora.Core.DTOs;
using MakiMora.Core.Services;
using Microsoft.AspNetCore.Authorization;

namespace MakiMora.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        private readonly ILocationService _locationService;

        public DashboardController(
            IOrderService orderService,
            IUserService userService,
            IProductService productService,
            ILocationService locationService)
        {
            _orderService = orderService;
            _userService = userService;
            _productService = productService;
            _locationService = locationService;
        }

        [HttpGet("stats")]
        [Authorize(Roles = "manager,hr")]
        public async Task<ActionResult<object>> GetDashboardStats([FromQuery] Guid? locationId)
        {
            var stats = new
            {
                TotalOrders = await GetTotalOrdersCount(locationId),
                ActiveOrders = await GetActiveOrdersCount(locationId),
                TodayOrders = await GetTodayOrdersCount(locationId),
                TotalProducts = await GetTotalProductsCount(locationId),
                AvailableProducts = await GetAvailableProductsCount(locationId),
                TotalCouriers = await GetTotalCouriersCount(locationId),
                TotalChefs = await GetTotalChefsCount(locationId)
            };

            return Ok(stats);
        }

        [HttpGet("recent-orders")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetRecentOrders([FromQuery] Guid? locationId, [FromQuery] int limit = 10)
        {
            IEnumerable<OrderDto> orders;

            if (locationId.HasValue)
            {
                orders = await _orderService.GetOrdersByLocationAsync(locationId.Value);
            }
            else
            {
                orders = await _orderService.GetOrdersAsync();
            }

            var recentOrders = orders
                .OrderByDescending(o => o.CreatedAt)
                .Take(limit);

            return Ok(recentOrders);
        }

        [HttpGet("orders-by-status")]
        [Authorize(Roles = "manager,hr")]
        public async Task<ActionResult<object>> GetOrdersByStatus([FromQuery] Guid? locationId)
        {
            var allOrders = locationId.HasValue
                ? await _orderService.GetOrdersByLocationAsync(locationId.Value)
                : await _orderService.GetOrdersAsync();

            var ordersByStatus = allOrders
                .GroupBy(o => o.Status.Name)
                .Select(g => new { Status = g.Key, Count = g.Count() });

            return Ok(ordersByStatus);
        }

        private async Task<int> GetTotalOrdersCount(Guid? locationId)
        {
            var orders = locationId.HasValue
                ? await _orderService.GetOrdersByLocationAsync(locationId.Value)
                : await _orderService.GetOrdersAsync();
            
            return orders.Count();
        }

        private async Task<int> GetActiveOrdersCount(Guid? locationId)
        {
            var pendingStatus = await GetStatusIdByName("pending");
            var preparingStatus = await GetStatusIdByName("preparing");
            var readyStatus = await GetStatusIdByName("ready");
            var assembledStatus = await GetStatusIdByName("assembled");
            var pickedUpStatus = await GetStatusIdByName("picked_up");

            var orders = locationId.HasValue
                ? await _orderService.GetOrdersByLocationAsync(locationId.Value)
                : await _orderService.GetOrdersAsync();

            var activeOrders = orders.Where(o => 
                o.Status.Id == pendingStatus || 
                o.Status.Id == preparingStatus || 
                o.Status.Id == readyStatus ||
                o.Status.Id == assembledStatus ||
                o.Status.Id == pickedUpStatus
            );

            return activeOrders.Count();
        }

        private async Task<int> GetTodayOrdersCount(Guid? locationId)
        {
            var today = DateTime.Today;
            var orders = locationId.HasValue
                ? await _orderService.GetOrdersByLocationAsync(locationId.Value)
                : await _orderService.GetOrdersAsync();

            var todayOrders = orders.Where(o => 
                o.CreatedAt.Date == today);

            return todayOrders.Count();
        }

        private async Task<int> GetTotalProductsCount(Guid? locationId)
        {
            var products = locationId.HasValue
                ? await _productService.GetProductsByLocationAsync(locationId.Value)
                : await _productService.GetProductsAsync();

            return products.Count();
        }

        private async Task<int> GetAvailableProductsCount(Guid? locationId)
        {
            var products = locationId.HasValue
                ? await _productService.GetProductsByLocationAsync(locationId.Value)
                : await _productService.GetAvailableProductsAsync();

            return products.Count(p => p.IsAvailable && !p.IsOnStopList);
        }

        private async Task<int> GetTotalCouriersCount(Guid? locationId)
        {
            var couriers = await _userService.GetUsersByRoleAsync("courier");
            if (locationId.HasValue)
            {
                couriers = couriers.Where(c => c.Locations.Any(l => l.Id == locationId.Value));
            }
            
            return couriers.Count();
        }

        private async Task<int> GetTotalChefsCount(Guid? locationId)
        {
            var chefs = await _userService.GetUsersByRoleAsync("sushi_chef");
            if (locationId.HasValue)
            {
                chefs = chefs.Where(c => c.Locations.Any(l => l.Id == locationId.Value));
            }
            
            return chefs.Count();
        }

        private async Task<Guid> GetStatusIdByName(string statusName)
        {
            // In a real implementation, we would get this from the database
            // For demo purposes, we'll return some default GUIDs
            return statusName switch
            {
                "pending" => Guid.Parse("11111111-1111-1111-1111-11111111"),
                "preparing" => Guid.Parse("2222-2222-2222-2222-2222"),
                "ready" => Guid.Parse("33333333-3333-3333-3333-33333333"),
                "assembled" => Guid.Parse("44444444-4444-4444-4444-44444444"),
                "picked_up" => Guid.Parse("55555555-5555-5555-5555-55555555"),
                "delivered" => Guid.Parse("66666666-6666-6666-6666-66666666"),
                "cancelled" => Guid.Parse("7777-7777-7777-7777-7777"),
                _ => Guid.Empty
            };
        }
    }
}