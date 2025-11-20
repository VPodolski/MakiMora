using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);
        Task<IEnumerable<Product>> GetByLocationAsync(Guid locationId);
        Task<IEnumerable<Product>> GetAvailableAsync();
        Task<IEnumerable<Product>> GetByAvailabilityAsync(bool isAvailable);
        Task<IEnumerable<Product>> GetByLocationAndCategoryAsync(Guid locationId, Guid categoryId);
        Task<IEnumerable<Product>> GetOnStopListAsync();
    }
}