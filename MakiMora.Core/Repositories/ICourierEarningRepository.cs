using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface ICourierEarningRepository : IRepository<CourierEarning>
    {
        Task<IEnumerable<CourierEarning>> GetByCourierAsync(Guid courierId);
        Task<IEnumerable<CourierEarning>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<CourierEarning>> GetByOrderAsync(Guid orderId);
        Task<IEnumerable<CourierEarning>> GetByTypeAsync(string earningType);
        Task<decimal> GetTotalEarningsByCourierAsync(Guid courierId, DateTime startDate, DateTime endDate);
    }
}