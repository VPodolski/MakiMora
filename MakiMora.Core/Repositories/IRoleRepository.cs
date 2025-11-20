using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetByUserIdAsync(Guid userId);
    }
}