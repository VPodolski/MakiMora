using AutoMapper;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Core.Services;

namespace MakiMora.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public CategoryService(
            ICategoryRepository categoryRepository,
            ILocationRepository locationRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return null;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => _mapper.Map<CategoryDto>(c));
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesByLocationAsync(Guid locationId)
        {
            var categories = await _categoryRepository.GetByLocationAsync(locationId);
            return categories.Select(c => _mapper.Map<CategoryDto>(c));
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequestDto createCategoryDto)
        {
            var location = await _locationRepository.GetByIdAsync(createCategoryDto.LocationId);
            if (location == null)
                throw new ArgumentException($"Location with id '{createCategoryDto.LocationId}' not found");

            var category = new Category
            {
                Name = createCategoryDto.Name,
                Description = createCategoryDto.Description,
                LocationId = createCategoryDto.LocationId,
                IsActive = createCategoryDto.IsActive,
                SortOrder = createCategoryDto.SortOrder
            };

            var createdCategory = await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryDto>(createdCategory);
        }

        public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryRequestDto updateCategoryDto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(id);
            if (existingCategory == null)
                throw new ArgumentException($"Category with id '{id}' not found");

            var location = await _locationRepository.GetByIdAsync(updateCategoryDto.LocationId);
            if (location == null)
                throw new ArgumentException($"Location with id '{updateCategoryDto.LocationId}' not found");

            existingCategory.Name = updateCategoryDto.Name;
            existingCategory.Description = updateCategoryDto.Description;
            existingCategory.LocationId = updateCategoryDto.LocationId;
            existingCategory.IsActive = updateCategoryDto.IsActive;
            existingCategory.SortOrder = updateCategoryDto.SortOrder;
            existingCategory.UpdatedAt = DateTime.UtcNow;

            var updatedCategory = await _categoryRepository.UpdateAsync(existingCategory);
            return _mapper.Map<CategoryDto>(updatedCategory);
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return false;

            await _categoryRepository.DeleteAsync(category);
            return true;
        }

        public async Task<IEnumerable<CategoryDto>> GetCategoriesWithProductsAsync(Guid locationId)
        {
            var categories = await _categoryRepository.GetByLocationWithProductsAsync(locationId);
            return categories.Select(c => _mapper.Map<CategoryDto>(c));
        }
    }
}