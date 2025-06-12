using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Core.Interfaces
{
    /// <summary>
    /// Service interface for Customer business operations
    /// </summary>
    public interface ICustomerService
    {
        // Read operations
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto?> GetCustomerByIdAsync(int customerId);
        Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm);
        Task<IEnumerable<CustomerDto>> GetIndividualCustomersAsync();
        Task<IEnumerable<CustomerDto>> GetStoreCustomersAsync();
        Task<IEnumerable<CustomerDto>> GetCustomersByTerritoryAsync(int territoryId);

        // Write operations
        Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto customerDto);
        Task<CustomerDto> UpdateCustomerAsync(CustomerUpdateDto customerDto);
        Task<bool> DeleteCustomerAsync(int customerId);

        // Business logic operations
        Task<bool> CustomerExistsAsync(int customerId);
        Task<bool> CanDeleteCustomerAsync(int customerId);
        Task<string> GenerateAccountNumberAsync();

        // Customer analytics
        Task<CustomerSummaryDto> GetCustomerSummaryAsync(int customerId);
        Task<IEnumerable<SalesOrderHeaderDto>> GetCustomerOrdersAsync(int customerId);
        Task<decimal> GetCustomerTotalSalesAsync(int customerId);
        Task<DateTime?> GetCustomerLastOrderDateAsync(int customerId);
        Task<IEnumerable<CustomerDto>> GetTopCustomersAsync(int count = 10);
    }

    // Additional DTOs for Customer service
    public class CustomerCreateDto
    {
        public int? PersonId { get; set; }
        public int? StoreId { get; set; }
        public int? TerritoryId { get; set; }
    }

    public class CustomerUpdateDto : CustomerCreateDto
    {
        public int CustomerId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
    }

    public class CustomerSummaryDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerType { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public int TotalOrders { get; set; }
        public decimal TotalSales { get; set; }
        public DateTime? LastOrderDate { get; set; }
        public DateTime? FirstOrderDate { get; set; }
        public string TerritoryName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
