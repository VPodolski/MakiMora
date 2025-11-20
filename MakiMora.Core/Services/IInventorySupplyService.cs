using MakiMora.Core.Entities;

namespace MakiMora.Core.Services
{
    public interface IInventorySupplyService
    {
        Task<InventorySupplyDto?> GetSupplyByIdAsync(Guid id);
        Task<IEnumerable<InventorySupplyDto>> GetSuppliesAsync();
        Task<IEnumerable<InventorySupplyDto>> GetSuppliesByLocationAsync(Guid locationId);
        Task<IEnumerable<InventorySupplyDto>> GetSuppliesByManagerAsync(Guid managerId);
        Task<IEnumerable<InventorySupplyDto>> GetSuppliesByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<InventorySupplyDto> CreateSupplyAsync(CreateInventorySupplyRequestDto createSupplyDto);
        Task<InventorySupplyDto> UpdateSupplyAsync(Guid id, UpdateInventorySupplyRequestDto updateSupplyDto);
        Task<bool> DeleteSupplyAsync(Guid id);
        Task<InventorySupplyDto> UpdateSupplyStatusAsync(Guid id, string status);
    }
}