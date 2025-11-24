using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(Guid statusId)
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .Where(o => o.StatusId == statusId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByLocationAsync(Guid locationId)
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .Where(o => o.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersWithItemsAsync()
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByStatusAndLocationAsync(Guid statusId, Guid locationId)
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .Where(o => o.StatusId == statusId && o.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByCourierAsync(Guid courierId)
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .Where(o => o.CourierId == courierId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByCustomerAsync(string customerName)
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .Where(o => o.CustomerName.Contains(customerName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .Where(o => o.CreatedAt >= startDate && o.CreatedAt <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetOrdersWithItemsAsync()
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByStatusAndLocationAsync(Guid statusId, Guid locationId)
        {
            return await _dbSet
                .Include(o => o.Location)
                .Include(o => o.Status)
                .Include(o => o.Courier)
                .Include(o => o.Assembler)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Items)
                    .ThenInclude(oi => oi.Status)
                .Where(o => o.StatusId == statusId && o.LocationId == locationId)
                .ToListAsync();
        }
    }
}