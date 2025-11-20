using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class OrderStatusRepository : Repository<OrderStatus>, IOrderStatusRepository
    {
        public OrderStatusRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<OrderStatus?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(os => os.Name == name);
        }

        public async Task<IEnumerable<OrderStatus>> GetActiveStatusesAsync()
        {
            return await _dbSet
                .Where(os => os.IsActive)
                .OrderBy(os => os.SortOrder)
                .ToListAsync();
        }
    }
}