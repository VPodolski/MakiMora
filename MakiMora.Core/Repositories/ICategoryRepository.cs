using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetByLocationAsync(Guid locationId);
        Task<IEnumerable<Category>> GetByLocationWithProductsAsync(Guid locationId);
        Task<Category?> GetByNameAndLocationAsync(string name, Guid locationId);
    }
}