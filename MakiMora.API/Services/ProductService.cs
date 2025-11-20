using AutoMapper;
using MakiMora.Core.Entities;
using MakiMora.Core.Repositories;
using MakiMora.Core.Services;

namespace MakiMora.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            ILocationRepository locationRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _locationRepository = locationRepository;
            _mapper = mapper;
        }

        public async Task<ProductDto?> GetProductByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return null;

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.Select(p => _mapper.Map<ProductDto>(p));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryAsync(Guid categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.Select(p => _mapper.Map<ProductDto>(p));
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByLocationAsync(Guid locationId)
        {
            var products = await _productRepository.GetByLocationAsync(locationId);
            return products.Select(p => _mapper.Map<ProductDto>(p));
        }

        public async Task<IEnumerable<ProductDto>> GetAvailableProductsAsync()
        {
            var products = await _productRepository.GetAvailableAsync();
            return products.Select(p => _mapper.Map<ProductDto>(p));
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductRequestDto createProductDto)
        {
            var category = await _categoryRepository.GetByIdAsync(createProductDto.CategoryId);
            if (category == null)
                throw new ArgumentException($"Category with id '{createProductDto.CategoryId}' not found");

            var location = await _locationRepository.GetByIdAsync(createProductDto.LocationId);
            if (location == null)
                throw new ArgumentException($"Location with id '{createProductDto.LocationId}' not found");

            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                CategoryId = createProductDto.CategoryId,
                LocationId = createProductDto.LocationId,
                ImageUrl = createProductDto.ImageUrl,
                PreparationTime = createProductDto.PreparationTime,
                IsAvailable = createProductDto.IsAvailable,
                IsOnStopList = false // Default to not on stop list
            };

            var createdProduct = await _productRepository.AddAsync(product);
            return _mapper.Map<ProductDto>(createdProduct);
        }

        public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductRequestDto updateProductDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
                throw new ArgumentException($"Product with id '{id}' not found");

            var category = await _categoryRepository.GetByIdAsync(updateProductDto.CategoryId);
            if (category == null)
                throw new ArgumentException($"Category with id '{updateProductDto.CategoryId}' not found");

            existingProduct.Name = updateProductDto.Name;
            existingProduct.Description = updateProductDto.Description;
            existingProduct.Price = updateProductDto.Price;
            existingProduct.CategoryId = updateProductDto.CategoryId;
            existingProduct.ImageUrl = updateProductDto.ImageUrl;
            existingProduct.PreparationTime = updateProductDto.PreparationTime;
            existingProduct.IsAvailable = updateProductDto.IsAvailable;

            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
            return _mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null) return false;

            await _productRepository.DeleteAsync(product);
            return true;
        }

        public async Task<ProductDto> UpdateProductAvailabilityAsync(Guid id, bool isAvailable)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new ArgumentException($"Product with id '{id}' not found");

            product.IsAvailable = isAvailable;
            var updatedProduct = await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<ProductDto> UpdateProductStopListStatusAsync(Guid id, bool isOnStopList)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
                throw new ArgumentException($"Product with id '{id}' not found");

            product.IsOnStopList = isOnStopList;
            var updatedProduct = await _productRepository.UpdateAsync(product);
            return _mapper.Map<ProductDto>(updatedProduct);
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByLocationAndCategoryAsync(Guid locationId, Guid categoryId)
        {
            var products = await _productRepository.GetByLocationAndCategoryAsync(locationId, categoryId);
            return products.Select(p => _mapper.Map<ProductDto>(p));
        }

        public async Task<IEnumerable<ProductDto>> GetOnStopListProductsAsync()
        {
            var products = await _productRepository.GetOnStopListAsync();
            return products.Select(p => _mapper.Map<ProductDto>(p));
        }
    }
}