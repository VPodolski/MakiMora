using AutoMapper;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Core.Services;

namespace MakiMora.API.Services
{
    public class InventorySupplyService : IInventorySupplyService
    {
        private readonly IInventorySupplyRepository _supplyRepository;
        private readonly IInventorySupplyItemRepository _supplyItemRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public InventorySupplyService(
            IInventorySupplyRepository supplyRepository,
            IInventorySupplyItemRepository supplyItemRepository,
            ILocationRepository locationRepository,
            IUserRepository userRepository,
            IProductRepository productRepository,
            IMapper mapper)
        {
            _supplyRepository = supplyRepository;
            _supplyItemRepository = supplyItemRepository;
            _locationRepository = locationRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<InventorySupplyDto?> GetSupplyByIdAsync(Guid id)
        {
            var supply = await _supplyRepository.GetByIdAsync(id);
            if (supply == null) return null;

            return _mapper.Map<InventorySupplyDto>(supply);
        }

        public async Task<IEnumerable<InventorySupplyDto>> GetSuppliesAsync()
        {
            var supplies = await _supplyRepository.GetAllAsync();
            return supplies.Select(s => _mapper.Map<InventorySupplyDto>(s));
        }

        public async Task<IEnumerable<InventorySupplyDto>> GetSuppliesByLocationAsync(Guid locationId)
        {
            var supplies = await _supplyRepository.GetByLocationAsync(locationId);
            return supplies.Select(s => _mapper.Map<InventorySupplyDto>(s));
        }

        public async Task<IEnumerable<InventorySupplyDto>> GetSuppliesByManagerAsync(Guid managerId)
        {
            var supplies = await _supplyRepository.GetByManagerAsync(managerId);
            return supplies.Select(s => _mapper.Map<InventorySupplyDto>(s));
        }

        public async Task<IEnumerable<InventorySupplyDto>> GetSuppliesByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var supplies = await _supplyRepository.GetByDateRangeAsync(startDate, endDate);
            return supplies.Select(s => _mapper.Map<InventorySupplyDto>(s));
        }

        public async Task<InventorySupplyDto> CreateSupplyAsync(CreateInventorySupplyRequestDto createSupplyDto)
        {
            var location = await _locationRepository.GetByIdAsync(createSupplyDto.LocationId);
            if (location == null)
                throw new ArgumentException($"Location with id '{createSupplyDto.LocationId}' not found");

            var manager = await _userRepository.GetByIdAsync(createSupplyDto.ManagerId);
            if (manager == null)
                throw new ArgumentException($"Manager with id '{createSupplyDto.ManagerId}' not found");

            var supply = new InventorySupply
            {
                LocationId = createSupplyDto.LocationId,
                SupplierName = createSupplyDto.SupplierName,
                SupplyDate = createSupplyDto.SupplyDate,
                ExpectedDate = createSupplyDto.ExpectedDate,
                Status = createSupplyDto.Status ?? "pending",
                ManagerId = createSupplyDto.ManagerId
            };

            var createdSupply = await _supplyRepository.AddAsync(supply);

            decimal totalCost = 0;

            // Add supply items
            foreach (var itemDto in createSupplyDto.Items)
            {
                Product? product = null;
                if (itemDto.ProductId.HasValue)
                {
                    product = await _productRepository.GetByIdAsync(itemDto.ProductId.Value);
                    if (product == null)
                        throw new ArgumentException($"Product with id '{itemDto.ProductId.Value}' not found");
                }

                var supplyItem = new InventorySupplyItem
                {
                    SupplyId = createdSupply.Id,
                    ProductId = itemDto.ProductId,
                    ProductName = product?.Name ?? itemDto.ProductName,
                    Quantity = itemDto.Quantity,
                    UnitCost = itemDto.UnitCost,
                    TotalCost = itemDto.UnitCost * itemDto.Quantity
                };

                await _supplyItemRepository.AddAsync(supplyItem);
                totalCost += supplyItem.TotalCost;
            }

            // Update total cost
            createdSupply.TotalCost = totalCost;
            await _supplyRepository.UpdateAsync(createdSupply);

            return _mapper.Map<InventorySupplyDto>(createdSupply);
        }

        public async Task<InventorySupplyDto> UpdateSupplyAsync(Guid id, UpdateInventorySupplyRequestDto updateSupplyDto)
        {
            var existingSupply = await _supplyRepository.GetByIdAsync(id);
            if (existingSupply == null)
                throw new ArgumentException($"Supply with id '{id}' not found");

            var location = await _locationRepository.GetByIdAsync(updateSupplyDto.LocationId);
            if (location == null)
                throw new ArgumentException($"Location with id '{updateSupplyDto.LocationId}' not found");

            var manager = await _userRepository.GetByIdAsync(updateSupplyDto.ManagerId);
            if (manager == null)
                throw new ArgumentException($"Manager with id '{updateSupplyDto.ManagerId}' not found");

            existingSupply.SupplierName = updateSupplyDto.SupplierName;
            existingSupply.SupplyDate = updateSupplyDto.SupplyDate;
            existingSupply.ExpectedDate = updateSupplyDto.ExpectedDate;
            existingSupply.Status = updateSupplyDto.Status;
            existingSupply.ManagerId = updateSupplyDto.ManagerId;

            var updatedSupply = await _supplyRepository.UpdateAsync(existingSupply);
            return _mapper.Map<InventorySupplyDto>(updatedSupply);
        }

        public async Task<bool> DeleteSupplyAsync(Guid id)
        {
            var supply = await _supplyRepository.GetByIdAsync(id);
            if (supply == null) return false;

            await _supplyRepository.DeleteAsync(supply);
            return true;
        }

        public async Task<InventorySupplyDto> UpdateSupplyStatusAsync(Guid id, string status)
        {
            var supply = await _supplyRepository.GetByIdAsync(id);
            if (supply == null)
                throw new ArgumentException($"Supply with id '{id}' not found");

            supply.Status = status;
            supply.UpdatedAt = DateTime.UtcNow;

            var updatedSupply = await _supplyRepository.UpdateAsync(supply);
            return _mapper.Map<InventorySupplyDto>(updatedSupply);
        }
    }
}