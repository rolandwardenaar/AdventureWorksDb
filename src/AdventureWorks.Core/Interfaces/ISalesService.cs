using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Core.Interfaces
{
    /// <summary>
    /// Service interface for Sales business operations
    /// </summary>
    public interface ISalesService
    {
        // Sales Order operations
        Task<IEnumerable<SalesOrderHeaderDto>> GetAllOrdersAsync();
        Task<SalesOrderHeaderDto?> GetOrderByIdAsync(int salesOrderId);
        Task<IEnumerable<SalesOrderHeaderDto>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<SalesOrderHeaderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<SalesOrderHeaderDto>> GetOrdersByStatusAsync(byte status);
        Task<IEnumerable<SalesOrderHeaderDto>> GetOrdersBySalesPersonAsync(int salesPersonId);

        // Write operations
        Task<SalesOrderHeaderDto> CreateOrderAsync(SalesOrderCreateDto orderDto);
        Task<SalesOrderHeaderDto> UpdateOrderAsync(SalesOrderUpdateDto orderDto);
        Task<bool> DeleteOrderAsync(int salesOrderId);
        Task<bool> CancelOrderAsync(int salesOrderId);
        Task<bool> ShipOrderAsync(int salesOrderId, DateTime shipDate);

        // Order details operations
        Task<IEnumerable<SalesOrderDetailDto>> GetOrderDetailsAsync(int salesOrderId);
        Task<SalesOrderDetailDto> AddOrderDetailAsync(SalesOrderDetailCreateDto detailDto);
        Task<SalesOrderDetailDto> UpdateOrderDetailAsync(SalesOrderDetailUpdateDto detailDto);
        Task<bool> RemoveOrderDetailAsync(int salesOrderId, int salesOrderDetailId);

        // Business logic operations
        Task<bool> OrderExistsAsync(int salesOrderId);
        Task<bool> CanModifyOrderAsync(int salesOrderId);
        Task<string> GenerateOrderNumberAsync();
        Task<decimal> CalculateOrderTotalAsync(int salesOrderId);
        Task<bool> ValidateOrderDetailsAsync(IEnumerable<SalesOrderDetailCreateDto> details);

        // Sales analytics
        Task<SalesSummaryDto> GetSalesSummaryAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<TopProductDto>> GetTopSellingProductsAsync(int count = 10, DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<SalesPersonPerformanceDto>> GetSalesPersonPerformanceAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<decimal> GetTotalSalesAsync(DateTime? startDate = null, DateTime? endDate = null);
        Task<IEnumerable<MonthlySalesDto>> GetMonthlySalesAsync(int year);
    }

    // Additional DTOs for Sales service
    public class SalesOrderCreateDto
    {
        public int CustomerId { get; set; }
        public int? SalesPersonId { get; set; }
        public int? TerritoryId { get; set; }
        public int BillToAddressId { get; set; }
        public int ShipToAddressId { get; set; }
        public int ShipMethodId { get; set; }
        public int? CreditCardId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime DueDate { get; set; }
        public string? PurchaseOrderNumber { get; set; }
        public string? Comment { get; set; }
        public List<SalesOrderDetailCreateDto> OrderDetails { get; set; } = new();
    }

    public class SalesOrderUpdateDto : SalesOrderCreateDto
    {
        public int SalesOrderId { get; set; }
        public byte RevisionNumber { get; set; }
        public string SalesOrderNumber { get; set; } = string.Empty;
    }

    public class SalesOrderDetailCreateDto
    {
        public int ProductId { get; set; }
        public int SpecialOfferId { get; set; } = 1; // Default to 'No Discount'
        public short OrderQty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceDiscount { get; set; } = 0;
        public string? CarrierTrackingNumber { get; set; }
    }

    public class SalesOrderDetailUpdateDto : SalesOrderDetailCreateDto
    {
        public int SalesOrderId { get; set; }
        public int SalesOrderDetailId { get; set; }
    }

    public class SalesSummaryDto
    {
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public int TotalCustomers { get; set; }
        public int TotalProducts { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductNumber { get; set; } = string.Empty;
        public int QuantitySold { get; set; }
        public decimal TotalSales { get; set; }
        public int OrderCount { get; set; }
    }

    public class SalesPersonPerformanceDto
    {
        public int BusinessEntityId { get; set; }
        public string SalesPersonName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
        public string TerritoryName { get; set; } = string.Empty;
    }

    public class MonthlySalesDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } = string.Empty;
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
