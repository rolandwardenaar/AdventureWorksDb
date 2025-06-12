using AdventureWorks.Core.Entities;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Infrastructure.Mappings
{
    /// <summary>
    /// Manual mapping methods between Product entity and DTOs
    /// </summary>
    public static class ProductMappings
    {
        public static ProductDto ToDto(this Product entity)
        {
            if (entity == null) return null!;

            return new ProductDto
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                ProductNumber = entity.ProductNumber,
                Color = entity.Color,
                ListPrice = entity.ListPrice,
                Size = entity.Size,
                Weight = entity.Weight,
                ProductCategoryId = entity.ProductSubcategory?.ProductCategoryId,
                ProductSubcategoryId = entity.ProductSubcategoryId,
                ProductModelId = entity.ProductModelId,
                SellStartDate = entity.SellStartDate,
                SellEndDate = entity.SellEndDate,
                DiscontinuedDate = entity.DiscontinuedDate,
                ModifiedDate = entity.ModifiedDate,
                // Navigation properties
                CategoryName = entity.ProductSubcategory?.ProductCategory?.Name,
                SubcategoryName = entity.ProductSubcategory?.Name,
                ModelName = entity.ProductModel?.Name
            };
        }

        public static ProductListDto ToListDto(this Product entity)
        {
            if (entity == null) return null!;

            return new ProductListDto
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                ProductNumber = entity.ProductNumber,
                ListPrice = entity.ListPrice,
                Color = entity.Color,
                CategoryName = entity.ProductSubcategory?.ProductCategory?.Name,
                SubcategoryName = entity.ProductSubcategory?.Name
            };
        }

        public static Product ToEntity(this ProductCreateDto dto)
        {
            if (dto == null) return null!;

            return new Product
            {
                Name = dto.Name,
                ProductNumber = dto.ProductNumber,
                Color = dto.Color,
                ListPrice = dto.ListPrice,
                Size = dto.Size,
                Weight = dto.Weight,
                ProductSubcategoryId = dto.ProductSubcategoryId,
                ProductModelId = dto.ProductModelId,
                SellStartDate = dto.SellStartDate,
                SellEndDate = dto.SellEndDate,
                ModifiedDate = DateTime.UtcNow,
                Rowguid = Guid.NewGuid(),
                MakeFlag = true,
                FinishedGoodsFlag = true,
                SafetyStockLevel = 1000,
                ReorderPoint = 750,
                StandardCost = dto.ListPrice * 0.6m, // Default standard cost
                DaysToManufacture = 1,
                Class = "H", // High
                Style = "M", // Medium
                ProductLine = "R" // Road
            };
        }

        public static void UpdateEntity(this ProductUpdateDto dto, Product entity)
        {
            if (dto == null || entity == null) return;

            entity.Name = dto.Name;
            entity.ProductNumber = dto.ProductNumber;
            entity.Color = dto.Color;
            entity.ListPrice = dto.ListPrice;
            entity.Size = dto.Size;
            entity.Weight = dto.Weight;
            entity.ProductSubcategoryId = dto.ProductSubcategoryId;
            entity.ProductModelId = dto.ProductModelId;
            entity.SellStartDate = dto.SellStartDate;
            entity.SellEndDate = dto.SellEndDate;
            entity.ModifiedDate = DateTime.UtcNow;
        }

        public static List<ProductDto> ToDto(this IEnumerable<Product> entities)
        {
            return entities?.Select(ToDto).ToList() ?? new List<ProductDto>();
        }

        public static List<ProductListDto> ToListDto(this IEnumerable<Product> entities)
        {
            return entities?.Select(ToListDto).ToList() ?? new List<ProductListDto>();
        }
    }
}
