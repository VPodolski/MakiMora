using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetByLocationAsync(Guid locationId)
        {
            return await _dbSet
                .Include(c => c.Location)
                .Where(c => c.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetByLocationWithProductsAsync(Guid locationId)
        {
            return await _dbSet
                .Include(c => c.Location)
                .Include(c => c.Products)
                .Where(c => c.LocationId == locationId)
                .ToListAsync();
        }

        public async Task<Category?> GetByNameAndLocationAsync(string name, Guid locationId)
        {
            return await _dbSet
                .Include(c => c.Location)
                .FirstOrDefaultAsync(c => c.Name == name && c.LocationId == locationId);
        }
    }
}