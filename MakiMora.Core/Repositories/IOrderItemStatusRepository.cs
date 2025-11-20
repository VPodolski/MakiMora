using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface IOrderItemStatusRepository : IRepository<OrderItemStatus>
    {
        Task<OrderItemStatus?> GetByNameAsync(string name);
        Task<IEnumerable<OrderItemStatus>> GetActiveStatusesAsync();
    }
}