using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface IInventorySupplyItemRepository : IRepository<InventorySupplyItem>
    {
        Task<IEnumerable<InventorySupplyItem>> GetBySupplyAsync(Guid supplyId);
        Task<IEnumerable<InventorySupplyItem>> GetByProductAsync(Guid productId);
    }
}