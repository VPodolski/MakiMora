using AutoMapper;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Core.Services;
using MakiMora.Core.DTOs;

namespace MakiMora.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IOrderItemStatusRepository _orderItemStatusRepository;
        private readonly IMapper _mapper;

        public OrderService(
            IOrderRepository orderRepository,
            IOrderItemRepository orderItemRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            ILocationRepository locationRepository,
            IOrderStatusRepository orderStatusRepository,
            IOrderItemStatusRepository orderItemStatusRepository,
            IMapper mapper)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _locationRepository = locationRepository;
            _orderStatusRepository = orderStatusRepository;
            _orderItemStatusRepository = orderItemStatusRepository;
            _mapper = mapper;
        }

        public async Task<OrderDto?> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return null;

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return orders.Select(o => _mapper.Map<OrderDto>(o));
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(Guid statusId)
        {
            var orders = await _orderRepository.GetByStatusAsync(statusId);
            return orders.Select(o => _mapper.Map<OrderDto>(o));
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByLocationAsync(Guid locationId)
        {
            var orders = await _orderRepository.GetByLocationAsync(locationId);
            return orders.Select(o => _mapper.Map<OrderDto>(o));
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCourierAsync(Guid courierId)
        {
            var orders = await _orderRepository.GetByCourierAsync(courierId);
            return orders.Select(o => _mapper.Map<OrderDto>(o));
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var orders = await _orderRepository.GetByDateRangeAsync(startDate, endDate);
            return orders.Select(o => _mapper.Map<OrderDto>(o));
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderRequestDto createOrderDto)
        {
            var location = await _locationRepository.GetByIdAsync(createOrderDto.LocationId); // Fixed: Using location repository
            if (location == null)
                throw new ArgumentException($"Location with id '{createOrderDto.LocationId}' not found");

            // Get initial 'pending' status
            var pendingStatus = await _orderStatusRepository.GetByNameAsync("pending");
            if (pendingStatus == null)
                throw new ArgumentException("Pending status not found");

            var order = new Order
            {
                OrderNumber = GenerateOrderNumber(),
                CustomerName = createOrderDto.CustomerName,
                CustomerPhone = createOrderDto.CustomerPhone,
                CustomerAddress = createOrderDto.CustomerAddress,
                LocationId = createOrderDto.LocationId,
                StatusId = pendingStatus.Id,
                TotalAmount = 0, // Will be calculated below
                DeliveryFee = createOrderDto.DeliveryFee,
                Comment = createOrderDto.Comment,
                DeliveryTime = createOrderDto.DeliveryTime
            };

            var createdOrder = await _orderRepository.AddAsync(order);

            decimal totalAmount = 0;

            // Add order items
            foreach (var itemDto in createOrderDto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    throw new ArgumentException($"Product with id '{itemDto.ProductId}' not found");

                // Get initial 'pending' status for order item
                var itemPendingStatus = await _orderItemStatusRepository.GetByNameAsync("pending");
                if (itemPendingStatus == null)
                    throw new ArgumentException("Pending status for order item not found");

                var orderItem = new OrderItem
                {
                    OrderId = createdOrder.Id,
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price,
                    TotalPrice = product.Price * itemDto.Quantity,
                    StatusId = itemPendingStatus.Id
                };

                await _orderItemRepository.AddAsync(orderItem);
                totalAmount += orderItem.TotalPrice;
            }

            // Update total amount
            createdOrder.TotalAmount = totalAmount;
            await _orderRepository.UpdateAsync(createdOrder);

            return _mapper.Map<OrderDto>(createdOrder);
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(Guid orderId, Guid statusId, string? note = null)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException($"Order with id '{orderId}' not found");

            var status = await _orderStatusRepository.GetByIdAsync(statusId);
            if (status == null)
                throw new ArgumentException($"Status with id '{statusId}' not found");

            order.StatusId = statusId;
            order.UpdatedAt = DateTime.UtcNow;

            // Add status history
            var statusHistory = new OrderStatusHistory
            {
                OrderId = orderId,
                StatusId = statusId,
                ChangedAt = DateTime.UtcNow,
                Note = note
            };

            var updatedOrder = await _orderRepository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(updatedOrder);
        }

        public async Task<OrderDto> AssignCourierToOrderAsync(Guid orderId, Guid courierId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException($"Order with id '{orderId}' not found");

            var courier = await _userRepository.GetByIdAsync(courierId);
            if (courier == null)
                throw new ArgumentException($"Courier with id '{courierId}' not found");

            order.CourierId = courierId;
            order.UpdatedAt = DateTime.UtcNow;

            var updatedOrder = await _orderRepository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(updatedOrder);
        }

        public async Task<OrderDto> UpdateOrderItemStatusAsync(Guid orderItemId, Guid statusId, string? note = null)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(orderItemId);
            if (orderItem == null)
                throw new ArgumentException($"Order item with id '{orderItemId}' not found");

            var status = await _orderItemStatusRepository.GetByIdAsync(statusId);
            if (status == null)
                throw new ArgumentException($"Status with id '{statusId}' not found");

            orderItem.StatusId = statusId;
            orderItem.UpdatedAt = DateTime.UtcNow;

            // Add status history
            var statusHistory = new OrderItemStatusHistory
            {
                OrderItemId = orderItemId,
                StatusId = statusId,
                ChangedAt = DateTime.UtcNow,
                Note = note
            };

            var updatedOrderItem = await _orderItemRepository.UpdateAsync(orderItem);

            // Update the parent order to reflect the change
            var order = await _orderRepository.GetByIdAsync(updatedOrderItem.OrderId);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> MarkOrderAsReadyAsync(Guid orderId)
        {
            var readyStatus = await _orderStatusRepository.GetByNameAsync("ready");
            if (readyStatus == null)
                throw new ArgumentException("Ready status not found");

            return await UpdateOrderStatusAsync(orderId, readyStatus.Id, "Order marked as ready for assembly");
        }

        public async Task<OrderDto> MarkOrderAsPickedUpAsync(Guid orderId, Guid courierId)
        {
            var pickedUpStatus = await _orderStatusRepository.GetByNameAsync("picked_up");
            if (pickedUpStatus == null)
                throw new ArgumentException("Picked up status not found");

            var order = await AssignCourierToOrderAsync(orderId, courierId);
            return await UpdateOrderStatusAsync(orderId, pickedUpStatus.Id, "Order picked up by courier");
        }

        public async Task<OrderDto> MarkOrderAsDeliveredAsync(Guid orderId)
        {
            var deliveredStatus = await _orderStatusRepository.GetByNameAsync("delivered");
            if (deliveredStatus == null)
                throw new ArgumentException("Delivered status not found");

            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException($"Order with id '{orderId}' not found");

            order.StatusId = deliveredStatus.Id;
            order.CompletedAt = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;

            var updatedOrder = await _orderRepository.UpdateAsync(order);
            return _mapper.Map<OrderDto>(updatedOrder);
        }

        public async Task<OrderDto> MarkOrderAsAssembledAsync(Guid orderId)
        {
            var assembledStatus = await _orderStatusRepository.GetByNameAsync("assembled");
            if (assembledStatus == null)
                throw new ArgumentException("Assembled status not found");

            return await UpdateOrderStatusAsync(orderId, assembledStatus.Id, "Order marked as assembled/packed");
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAndLocationAsync(Guid statusId, Guid locationId)
        {
            var orders = await _orderRepository.GetByStatusAndLocationAsync(statusId, locationId);
            return orders.Select(o => _mapper.Map<OrderDto>(o));
        }

        private string GenerateOrderNumber()
        {
            // Generate a unique order number (in real implementation, this should be more sophisticated)
            return $"ORD{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(100, 999)}";
        }
    }
}