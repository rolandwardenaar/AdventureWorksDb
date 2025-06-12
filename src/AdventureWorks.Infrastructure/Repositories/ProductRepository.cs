using Microsoft.EntityFrameworkCore;
using AdventureWorks.Core.Interfaces;
using AdventureWorks.Infrastructure.Data;
using AdventureWorks.Core.Entities;

namespace AdventureWorks.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public ProductRepository(AdventureWorksDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(int categoryId)
    {
        return await _dbSet
            .Include(p => p.ProductSubcategory)
                .ThenInclude(ps => ps!.ProductCategory)
            .Where(p => p.ProductSubcategory != null && 
                       p.ProductSubcategory.ProductCategoryId == categoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetBySubcategoryAsync(int subcategoryId)
    {
        return await _dbSet
            .Include(p => p.ProductSubcategory)
            .Where(p => p.ProductSubcategoryId == subcategoryId)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchByNameAsync(string name)
    {
        return await _dbSet
            .Where(p => p.Name.Contains(name))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _dbSet
            .Where(p => p.ListPrice >= minPrice && p.ListPrice <= maxPrice)
            .OrderBy(p => p.ListPrice)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _dbSet
            .Where(p => p.SellEndDate == null || p.SellEndDate > DateTime.Now)
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public override async Task<Product?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.ProductSubcategory)
                .ThenInclude(ps => ps!.ProductCategory)
            .Include(p => p.ProductModel)
            .Include(p => p.ProductInventories)
                .ThenInclude(pi => pi.Location)
            .FirstOrDefaultAsync(p => p.ProductId == id);
    }
}
