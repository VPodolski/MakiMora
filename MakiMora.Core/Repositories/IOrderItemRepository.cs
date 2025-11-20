using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem>
    {
        Task<IEnumerable<OrderItem>> GetByOrderAsync(Guid orderId);
        Task<IEnumerable<OrderItem>> GetByProductAsync(Guid productId);
        Task<IEnumerable<OrderItem>> GetByStatusAsync(Guid statusId);
        Task<IEnumerable<OrderItem>> GetByOrderAndStatusAsync(Guid orderId, Guid statusId);
        Task<IEnumerable<OrderItem>> GetByCourierAsync(Guid courierId);
        Task<IEnumerable<OrderItem>> GetByPreparedByAsync(Guid preparedById);
    }
}