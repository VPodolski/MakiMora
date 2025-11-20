using Microsoft.EntityFrameworkCore;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Infrastructure.Data;

namespace MakiMora.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MakiMoraDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(Guid roleId)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .Where(u => u.UserRoles.Any(ur => ur.RoleId == roleId))
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByLocationAsync(Guid locationId)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .Where(u => u.UserLocations.Any(ul => ul.LocationId == locationId))
                .ToListAsync();
        }

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _dbSet.AnyAsync(u => u.Username == username);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Include(u => u.UserLocations)
                    .ThenInclude(ul => ul.Location)
                .FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}