using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class InventorySupplyItemRepository : Repository<InventorySupplyItem>, IInventorySupplyItemRepository
    {
        public InventorySupplyItemRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<InventorySupplyItem>> GetBySupplyAsync(Guid supplyId)
        {
            return await _dbSet
                .Include(item => item.Supply)
                .Include(item => item.Product)
                .Where(item => item.SupplyId == supplyId)
                .ToListAsync();
        }

        public async Task<IEnumerable<InventorySupplyItem>> GetByProductAsync(Guid productId)
        {
            return await _dbSet
                .Include(item => item.Supply)
                .Include(item => item.Product)
                .Where(item => item.ProductId == productId)
                .ToListAsync();
        }
    }
}