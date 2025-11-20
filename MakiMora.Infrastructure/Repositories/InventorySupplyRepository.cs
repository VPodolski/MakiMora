using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class InventorySupplyRepository : Repository<InventorySupply>, IInventorySupplyRepository
    {
        public InventorySupplyRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InventorySupply>> GetByLocationAsync(Guid locationId)
        {
            return await _dbSet
                .Include(s => s.Location)
                .Include(s => s.Manager)
                .Include(s => s.Items)
                    .ThenInclude(item => item.Product)
                .Where(s => s.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventorySupply>> GetByManagerAsync(Guid managerId)
        {
            return await _dbSet
                .Include(s => s.Location)
                .Include(s => s.Manager)
                .Include(s => s.Items)
                    .ThenInclude(item => item.Product)
                .Where(s => s.ManagerId == managerId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventorySupply>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(s => s.Location)
                .Include(s => s.Manager)
                .Include(s => s.Items)
                    .ThenInclude(item => item.Product)
                .Where(s => s.SupplyDate >= startDate && s.SupplyDate <= endDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventorySupply>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(s => s.Location)
                .Include(s => s.Manager)
                .Include(s => s.Items)
                    .ThenInclude(item => item.Product)
                .Where(s => s.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventorySupply>> GetByLocationAndStatusAsync(Guid locationId, string status)
        {
            return await _dbSet
                .Include(s => s.Location)
                .Include(s => s.Manager)
                .Include(s => s.Items)
                    .ThenInclude(item => item.Product)
                .Where(s => s.LocationId == locationId && s.Status == status)
                .ToListAsync();
        }
    }
}