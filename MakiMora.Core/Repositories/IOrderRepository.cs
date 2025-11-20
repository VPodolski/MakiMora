using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByStatusAsync(Guid statusId);
        Task<IEnumerable<Order>> GetByLocationAsync(Guid locationId);
        Task<IEnumerable<Order>> GetByCourierAsync(Guid courierId);
        Task<IEnumerable<Order>> GetByCustomerAsync(string customerName);
        Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<Order>> GetOrdersWithItemsAsync();
        Task<IEnumerable<Order>> GetByStatusAndLocationAsync(Guid statusId, Guid locationId);
    }
}