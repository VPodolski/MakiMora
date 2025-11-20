using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByLocationAsync(Guid locationId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Where(p => p.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAvailableAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Where(p => p.IsAvailable && !p.IsOnStopList)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByAvailabilityAsync(bool isAvailable)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Where(p => p.IsAvailable == isAvailable)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByLocationAndCategoryAsync(Guid locationId, Guid categoryId)
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Where(p => p.LocationId == locationId && p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetOnStopListAsync()
        {
            return await _dbSet
                .Include(p => p.Category)
                .Include(p => p.Location)
                .Where(p => p.IsOnStopList)
                .ToListAsync();
        }
    }
}