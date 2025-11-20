using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface ILocationRepository : IRepository<Location>
    {
        Task<IEnumerable<Location>> GetByManagerAsync(Guid managerId);
        Task<Location?> GetByNameAsync(string name);
        Task<IEnumerable<Location>> GetWithUsersAsync();
    }
}