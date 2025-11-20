using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Location>> GetByManagerAsync(Guid managerId)
        {
            return await _dbSet
                .Include(l => l.UserLocations)
                    .ThenInclude(ul => ul.User)
                .Where(l => l.UserLocations.Any(ul => ul.UserId == managerId))
                .ToListAsync();
        }

        public async Task<Location?> GetByNameAsync(string name)
        {
            return await _dbSet
                .Include(l => l.UserLocations)
                    .ThenInclude(ul => ul.User)
                .FirstOrDefaultAsync(l => l.Name == name);
        }

        public async Task<IEnumerable<Location>> GetWithUsersAsync()
        {
            return await _dbSet
                .Include(l => l.UserLocations)
                    .ThenInclude(ul => ul.User)
                .ThenInclude(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .ToListAsync();
        }
    }
}