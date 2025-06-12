using AdventureWorks.Core.Interfaces;
using AdventureWorks.Infrastructure.Mappings;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Product business operations
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IGenericRepository<Core.Entities.ProductCategory> _categoryRepository;
        private readonly IGenericRepository<Core.Entities.ProductSubcategory> _subcategoryRepository;
        private readonly IGenericRepository<Core.Entities.ProductInventory> _inventoryRepository;

        public ProductService(
            IProductRepository productRepository,
            IGenericRepository<Core.Entities.ProductCategory> categoryRepository,
            IGenericRepository<Core.Entities.ProductSubcategory> subcategoryRepository,
            IGenericRepository<Core.Entities.ProductInventory> inventoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subcategoryRepository = subcategoryRepository;
            _inventoryRepository = inventoryRepository;
        }

        #region Read Operations

        public async Task<IEnumerable<ProductListDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return products.ToListDto();
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            return product?.ToDto();
        }

        public async Task<IEnumerable<ProductListDto>> SearchProductsAsync(string searchTerm)
        {
            var products = await _productRepository.SearchAsync(searchTerm);
            return products.ToListDto();
        }

        public async Task<IEnumerable<ProductListDto>> GetProductsByCategoryAsync(int categoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(categoryId);
            return products.ToListDto();
        }

        public async Task<IEnumerable<ProductListDto>> GetProductsBySubcategoryAsync(int subcategoryId)
        {
            var products = await _productRepository.GetBySubcategoryAsync(subcategoryId);
            return products.ToListDto();
        }

        public async Task<IEnumerable<ProductListDto>> GetActiveProductsAsync()
        {
            var products = await _productRepository.GetActiveProductsAsync();
            return products.ToListDto();
        }

        public async Task<IEnumerable<ProductListDto>> GetDiscontinuedProductsAsync()
        {
            var products = await _productRepository.GetDiscontinuedProductsAsync();
            return products.ToListDto();
        }

        #endregion

        #region Write Operations

        public async Task<ProductDto> CreateProductAsync(ProductCreateDto productDto)
        {
            var product = productDto.ToEntity();
            var createdProduct = await _productRepository.AddAsync(product);
            return createdProduct.ToDto();
        }

        public async Task<ProductDto> UpdateProductAsync(ProductUpdateDto productDto)
        {
            var existingProduct = await _productRepository.GetByIdAsync(productDto.ProductId);
            if (existingProduct == null)
                throw new ArgumentException($"Product with ID {productDto.ProductId} not found.");

            productDto.UpdateEntity(existingProduct);
            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
            return updatedProduct.ToDto();
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            if (!await CanDeleteProductAsync(productId))
                return false;

            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            return await _productRepository.DeleteAsync(product);
        }

        public async Task<bool> DiscontinueProductAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            product.DiscontinuedDate = DateTime.UtcNow;
            product.SellEndDate = DateTime.UtcNow;
            product.ModifiedDate = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> ReactivateProductAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return false;

            product.DiscontinuedDate = null;
            product.SellEndDate = null;
            product.ModifiedDate = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            return true;
        }

        #endregion

        #region Business Logic Operations

        public async Task<bool> ProductExistsAsync(int productId)
        {
            return await _productRepository.ExistsAsync(productId);
        }

        public async Task<bool> IsProductNumberUniqueAsync(string productNumber, int? excludeProductId = null)
        {
            var existingProduct = await _productRepository.FindFirstAsync(p => 
                p.ProductNumber == productNumber && 
                (excludeProductId == null || p.ProductId != excludeProductId));
            
            return existingProduct == null;
        }

        public async Task<bool> CanDeleteProductAsync(int productId)
        {
            // Check if product has any sales orders
            var hasOrders = await _productRepository.HasOrdersAsync(productId);
            return !hasOrders;
        }

        public async Task<decimal> CalculateStandardCostAsync(decimal listPrice)
        {
            // Business rule: Standard cost is typically 60% of list price
            return listPrice * 0.6m;
        }

        public async Task<bool> IsProductInStockAsync(int productId)
        {
            var stockLevel = await GetStockLevelAsync(productId);
            return stockLevel > 0;
        }

        #endregion

        #region Inventory Operations

        public async Task<int> GetStockLevelAsync(int productId)
        {
            var inventory = await _inventoryRepository.FindFirstAsync(i => i.ProductId == productId);
            return inventory?.Quantity ?? 0;
        }

        public async Task<bool> UpdateStockLevelAsync(int productId, int newStockLevel)
        {
            var inventory = await _inventoryRepository.FindFirstAsync(i => i.ProductId == productId);
            if (inventory == null)
                return false;

            inventory.Quantity = (short)newStockLevel;
            inventory.ModifiedDate = DateTime.UtcNow;

            await _inventoryRepository.UpdateAsync(inventory);
            return true;
        }

        public async Task<IEnumerable<ProductListDto>> GetLowStockProductsAsync()
        {
            var products = await _productRepository.GetLowStockProductsAsync();
            return products.ToListDto();
        }

        public async Task<IEnumerable<ProductListDto>> GetProductsToReorderAsync()
        {
            var products = await _productRepository.GetProductsToReorderAsync();
            return products.ToListDto();
        }

        #endregion

        #region Category Operations

        public async Task<IEnumerable<ProductCategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new ProductCategoryDto
            {
                ProductCategoryId = c.ProductCategoryId,
                Name = c.Name,
                SubcategoryCount = c.ProductSubcategories.Count()
            });
        }

        public async Task<IEnumerable<ProductSubcategoryDto>> GetSubcategoriesAsync(int? categoryId = null)
        {
            var subcategories = categoryId.HasValue
                ? await _subcategoryRepository.FindAsync(s => s.ProductCategoryId == categoryId.Value)
                : await _subcategoryRepository.GetAllAsync();

            return subcategories.Select(s => new ProductSubcategoryDto
            {
                ProductSubcategoryId = s.ProductSubcategoryId,
                Name = s.Name,
                ProductCategoryId = s.ProductCategoryId,
                CategoryName = s.ProductCategory?.Name ?? "",
                ProductCount = s.Products.Count()
            });
        }

        #endregion
    }
}
