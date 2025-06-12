using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Core.Interfaces
{
    /// <summary>
    /// Service interface for Product business operations
    /// </summary>
    public interface IProductService
    {
        // Read operations
        Task<IEnumerable<ProductListDto>> GetAllProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductListDto>> SearchProductsAsync(string searchTerm);
        Task<IEnumerable<ProductListDto>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<ProductListDto>> GetProductsBySubcategoryAsync(int subcategoryId);
        Task<IEnumerable<ProductListDto>> GetActiveProductsAsync();
        Task<IEnumerable<ProductListDto>> GetDiscontinuedProductsAsync();

        // Write operations
        Task<ProductDto> CreateProductAsync(ProductCreateDto productDto);
        Task<ProductDto> UpdateProductAsync(ProductUpdateDto productDto);
        Task<bool> DeleteProductAsync(int productId);
        Task<bool> DiscontinueProductAsync(int productId);
        Task<bool> ReactivateProductAsync(int productId);

        // Business logic operations
        Task<bool> ProductExistsAsync(int productId);
        Task<bool> IsProductNumberUniqueAsync(string productNumber, int? excludeProductId = null);
        Task<bool> CanDeleteProductAsync(int productId);
        Task<decimal> CalculateStandardCostAsync(decimal listPrice);
        Task<bool> IsProductInStockAsync(int productId);

        // Inventory operations
        Task<int> GetStockLevelAsync(int productId);
        Task<bool> UpdateStockLevelAsync(int productId, int newStockLevel);
        Task<IEnumerable<ProductListDto>> GetLowStockProductsAsync();
        Task<IEnumerable<ProductListDto>> GetProductsToReorderAsync();

        // Category operations
        Task<IEnumerable<ProductCategoryDto>> GetCategoriesAsync();
        Task<IEnumerable<ProductSubcategoryDto>> GetSubcategoriesAsync(int? categoryId = null);
    }

    // Additional DTOs needed for Product service
    public class ProductCategoryDto
    {
        public int ProductCategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SubcategoryCount { get; set; }
    }

    public class ProductSubcategoryDto
    {
        public int ProductSubcategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ProductCategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int ProductCount { get; set; }
    }
}
