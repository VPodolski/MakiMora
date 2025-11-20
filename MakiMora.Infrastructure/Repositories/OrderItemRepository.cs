using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderAsync(Guid orderId)
        {
            return await _dbSet
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Include(oi => oi.Status)
                .Include(oi => oi.PreparedBy)
                .Include(oi => oi.AssembledBy)
                .Where(oi => oi.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByProductAsync(Guid productId)
        {
            return await _dbSet
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Include(oi => oi.Status)
                .Include(oi => oi.PreparedBy)
                .Include(oi => oi.AssembledBy)
                .Where(oi => oi.ProductId == productId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByStatusAsync(Guid statusId)
        {
            return await _dbSet
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Include(oi => oi.Status)
                .Include(oi => oi.PreparedBy)
                .Include(oi => oi.AssembledBy)
                .Where(oi => oi.StatusId == statusId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByOrderAndStatusAsync(Guid orderId, Guid statusId)
        {
            return await _dbSet
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Include(oi => oi.Status)
                .Include(oi => oi.PreparedBy)
                .Include(oi => oi.AssembledBy)
                .Where(oi => oi.OrderId == orderId && oi.StatusId == statusId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByCourierAsync(Guid courierId)
        {
            return await _dbSet
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Include(oi => oi.Status)
                .Include(oi => oi.PreparedBy)
                .Include(oi => oi.AssembledBy)
                .Where(oi => oi.Order.CourierId == courierId)
                .ToListAsync();
        }

        public async Task<IEnumerable<OrderItem>> GetByPreparedByAsync(Guid preparedById)
        {
            return await _dbSet
                .Include(oi => oi.Order)
                .Include(oi => oi.Product)
                .Include(oi => oi.Status)
                .Include(oi => oi.PreparedBy)
                .Include(oi => oi.AssembledBy)
                .Where(oi => oi.PreparedById == preparedById)
                .ToListAsync();
        }
    }
}