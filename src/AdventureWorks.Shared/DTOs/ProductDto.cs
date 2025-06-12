namespace AdventureWorks.Shared.DTOs;

public class ProductDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public string? Color { get; set; }
    public decimal StandardCost { get; set; }
    public decimal ListPrice { get; set; }
    public string? Size { get; set; }
    public decimal? Weight { get; set; }
    public int? ProductCategoryId { get; set; }
    public int? ProductSubcategoryId { get; set; }
    public int? ProductModelId { get; set; }    public string? CategoryName { get; set; }
    public string? SubcategoryName { get; set; }
    public string? ModelName { get; set; }
    public int SafetyStockLevel { get; set; }
    public int ReorderPoint { get; set; }
    public DateTime SellStartDate { get; set; }
    public DateTime? SellEndDate { get; set; }
    public DateTime? DiscontinuedDate { get; set; }
    public bool IsActive => SellEndDate == null || SellEndDate > DateTime.Now;
    public DateTime ModifiedDate { get; set; }
}

public class ProductListDto
{
    public int ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public decimal ListPrice { get; set; }
    public string? Color { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string SubcategoryName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}

public class ProductCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string ProductNumber { get; set; } = string.Empty;
    public bool MakeFlag { get; set; } = true;
    public bool FinishedGoodsFlag { get; set; } = true;
    public string? Color { get; set; }
    public int SafetyStockLevel { get; set; }
    public int ReorderPoint { get; set; }
    public decimal StandardCost { get; set; }
    public decimal ListPrice { get; set; }
    public string? Size { get; set; }
    public decimal? Weight { get; set; }
    public int DaysToManufacture { get; set; }
    public int? ProductSubcategoryId { get; set; }
    public int? ProductModelId { get; set; }
    public DateTime SellStartDate { get; set; } = DateTime.Now;
    public DateTime? SellEndDate { get; set; }
}

public class ProductUpdateDto : ProductCreateDto
{
    public int ProductId { get; set; }
}
