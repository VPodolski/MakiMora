using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class CourierEarningRepository : Repository<CourierEarning>, ICourierEarningRepository
    {
        public CourierEarningRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CourierEarning>> GetByCourierAsync(Guid courierId)
        {
            return await _dbSet
                .Include(ce => ce.Courier)
                .Include(ce => ce.Order)
                .Where(ce => ce.CourierId == courierId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourierEarning>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(ce => ce.Courier)
                .Include(ce => ce.Order)
                .Where(ce => ce.Date >= startDate && ce.Date <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourierEarning>> GetByOrderAsync(Guid orderId)
        {
            return await _dbSet
                .Include(ce => ce.Courier)
                .Include(ce => ce.Order)
                .Where(ce => ce.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<CourierEarning>> GetByTypeAsync(string earningType)
        {
            return await _dbSet
                .Include(ce => ce.Courier)
                .Include(ce => ce.Order)
                .Where(ce => ce.EarningType == earningType)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalEarningsByCourierAsync(Guid courierId, DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Where(ce => ce.CourierId == courierId && ce.Date >= startDate && ce.Date <= endDate)
                .SumAsync(ce => ce.Amount);
        }
    }
}