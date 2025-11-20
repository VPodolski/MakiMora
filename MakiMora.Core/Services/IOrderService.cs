using MakiMora.Core.Entities;
using MakiMora.Core.DTOs;

namespace MakiMora.Core.Services
{
    public interface IOrderService
    {
        Task<OrderDto?> GetOrderByIdAsync(Guid id);
        Task<IEnumerable<OrderDto>> GetOrdersAsync();
        Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(Guid statusId);
        Task<IEnumerable<OrderDto>> GetOrdersByLocationAsync(Guid locationId);
        Task<IEnumerable<OrderDto>> GetOrdersByCourierAsync(Guid courierId);
        Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<OrderDto> CreateOrderAsync(CreateOrderRequestDto createOrderDto);
        Task<OrderDto> UpdateOrderStatusAsync(Guid orderId, Guid statusId, string? note = null);
        Task<OrderDto> AssignCourierToOrderAsync(Guid orderId, Guid courierId);
        Task<OrderDto> UpdateOrderItemStatusAsync(Guid orderItemId, Guid statusId, string? note = null);
        Task<OrderDto> MarkOrderAsReadyAsync(Guid orderId);
        Task<OrderDto> MarkOrderAsPickedUpAsync(Guid orderId, Guid courierId);
        Task<OrderDto> MarkOrderAsDeliveredAsync(Guid orderId);
        Task<IEnumerable<OrderDto>> GetOrdersByStatusAndLocationAsync(Guid statusId, Guid locationId);
    }
}