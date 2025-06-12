using AdventureWorks.Core.Entities;

namespace AdventureWorks.Core.Interfaces;

public interface IProductRepository : IGenericRepository<Product>
{
    Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId);
    Task<IEnumerable<Product>> GetBySubcategoryAsync(int subcategoryId);
    Task<IEnumerable<Product>> SearchByNameAsync(string name);
    Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
    Task<IEnumerable<Product>> GetActiveProductsAsync();
    
    // Additional methods needed for ProductService
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
    Task<IEnumerable<Product>> GetDiscontinuedProductsAsync();
    Task<bool> HasOrdersAsync(int productId);
    Task<IEnumerable<Product>> GetLowStockProductsAsync();
    Task<IEnumerable<Product>> GetProductsToReorderAsync();
}
