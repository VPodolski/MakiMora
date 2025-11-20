using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface IOrderStatusRepository : IRepository<OrderStatus>
    {
        Task<OrderStatus?> GetByNameAsync(string name);
        Task<IEnumerable<OrderStatus>> GetActiveStatusesAsync();
    }
}