using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class OrderItemStatusRepository : Repository<OrderItemStatus>, IOrderItemStatusRepository
    {
        public OrderItemStatusRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<OrderItemStatus?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(ois => ois.Name == name);
        }

        public async Task<IEnumerable<OrderItemStatus>> GetActiveStatusesAsync()
        {
            return await _dbSet
                .Where(ois => ois.IsActive)
                .OrderBy(ois => ois.SortOrder)
                .ToListAsync();
        }
    }
}