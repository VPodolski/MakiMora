using MakiMora.Core.Entities;

namespace MakiMora.Core.Repositories
{
    public interface IInventorySupplyRepository : IRepository<InventorySupply>
    {
        Task<IEnumerable<InventorySupply>> GetByLocationAsync(Guid locationId);
        Task<IEnumerable<InventorySupply>> GetByManagerAsync(Guid managerId);
        Task<IEnumerable<InventorySupply>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<InventorySupply>> GetByStatusAsync(string status);
        Task<IEnumerable<InventorySupply>> GetByLocationAndStatusAsync(Guid locationId, string status);
    }
}