using AdventureWorks.Core.Interfaces;
using AdventureWorks.Infrastructure.Mappings;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Customer business operations
    /// </summary>
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IGenericRepository<Core.Entities.SalesTerritory> _territoryRepository;

        public CustomerService(
            ICustomerRepository customerRepository,
            ISalesOrderRepository salesOrderRepository,
            IGenericRepository<Core.Entities.SalesTerritory> territoryRepository)
        {
            _customerRepository = customerRepository;
            _salesOrderRepository = salesOrderRepository;
            _territoryRepository = territoryRepository;
        }

        #region Read Operations

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.ToDto();
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            return customer?.ToDto();
        }

        public async Task<IEnumerable<CustomerDto>> SearchCustomersAsync(string searchTerm)
        {
            var customers = await _customerRepository.SearchCustomersAsync(searchTerm);
            return customers.ToDto();
        }

        public async Task<IEnumerable<CustomerDto>> GetIndividualCustomersAsync()
        {
            var customers = await _customerRepository.GetIndividualCustomersAsync();
            return customers.ToDto();
        }

        public async Task<IEnumerable<CustomerDto>> GetStoreCustomersAsync()
        {
            var customers = await _customerRepository.GetStoreCustomersAsync();
            return customers.ToDto();
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersByTerritoryAsync(int territoryId)
        {
            var customers = await _customerRepository.GetByTerritoryAsync(territoryId);
            return customers.ToDto();
        }

        #endregion

        #region Write Operations

        public async Task<CustomerDto> CreateCustomerAsync(CustomerCreateDto customerDto)
        {
            var customer = new Core.Entities.Customer
            {
                PersonId = customerDto.PersonId,
                StoreId = customerDto.StoreId,
                TerritoryId = customerDto.TerritoryId,
                AccountNumber = await GenerateAccountNumberAsync(),
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.UtcNow
            };

            var createdCustomer = await _customerRepository.AddAsync(customer);
            return createdCustomer.ToDto();
        }

        public async Task<CustomerDto> UpdateCustomerAsync(CustomerUpdateDto customerDto)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerDto.CustomerId);
            if (existingCustomer == null)
                throw new ArgumentException($"Customer with ID {customerDto.CustomerId} not found.");

            existingCustomer.PersonId = customerDto.PersonId;
            existingCustomer.StoreId = customerDto.StoreId;
            existingCustomer.TerritoryId = customerDto.TerritoryId;
            existingCustomer.AccountNumber = customerDto.AccountNumber;
            existingCustomer.ModifiedDate = DateTime.UtcNow;

            var updatedCustomer = await _customerRepository.UpdateAsync(existingCustomer);
            return updatedCustomer.ToDto();
        }

        public async Task<bool> DeleteCustomerAsync(int customerId)
        {
            if (!await CanDeleteCustomerAsync(customerId))
                return false;

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                return false;

            return await _customerRepository.DeleteAsync(customer);
        }

        #endregion

        #region Business Logic Operations

        public async Task<bool> CustomerExistsAsync(int customerId)
        {
            return await _customerRepository.ExistsAsync(customerId);
        }

        public async Task<bool> CanDeleteCustomerAsync(int customerId)
        {
            // Check if customer has any sales orders
            var orders = await _salesOrderRepository.GetByCustomerAsync(customerId);
            return !orders.Any();
        }

        public async Task<string> GenerateAccountNumberAsync()
        {
            var lastCustomer = await _customerRepository.GetLastCustomerAsync();
            var nextId = (lastCustomer?.CustomerId ?? 0) + 1;
            return $"AW{nextId:D8}";
        }

        #endregion

        #region Customer Analytics

        public async Task<CustomerSummaryDto> GetCustomerSummaryAsync(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new ArgumentException($"Customer with ID {customerId} not found.");

            var orders = await _salesOrderRepository.GetByCustomerAsync(customerId);
            var totalSales = orders.Sum(o => o.TotalDue);
            var lastOrderDate = orders.OrderByDescending(o => o.OrderDate).FirstOrDefault()?.OrderDate;
            var firstOrderDate = orders.OrderBy(o => o.OrderDate).FirstOrDefault()?.OrderDate;

            return new CustomerSummaryDto
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.Person?.FirstName + " " + customer.Person?.LastName ?? customer.Store?.Name ?? "Unknown",
                CustomerType = customer.PersonId.HasValue ? "Individual" : "Store",
                AccountNumber = customer.AccountNumber,
                TotalOrders = orders.Count(),
                TotalSales = totalSales,
                LastOrderDate = lastOrderDate,
                FirstOrderDate = firstOrderDate,
                TerritoryName = customer.Territory?.Name ?? "",
                IsActive = true // You can define business rules for this
            };
        }

        public async Task<IEnumerable<SalesOrderHeaderDto>> GetCustomerOrdersAsync(int customerId)
        {
            var orders = await _salesOrderRepository.GetByCustomerAsync(customerId);
            return orders.ToDto();
        }

        public async Task<decimal> GetCustomerTotalSalesAsync(int customerId)
        {
            var orders = await _salesOrderRepository.GetByCustomerAsync(customerId);
            return orders.Sum(o => o.TotalDue);
        }

        public async Task<DateTime?> GetCustomerLastOrderDateAsync(int customerId)
        {
            var orders = await _salesOrderRepository.GetByCustomerAsync(customerId);
            return orders.OrderByDescending(o => o.OrderDate).FirstOrDefault()?.OrderDate;
        }

        public async Task<IEnumerable<CustomerDto>> GetTopCustomersAsync(int count = 10)
        {
            var customers = await _customerRepository.GetTopCustomersAsync(count);
            return customers.ToDto();
        }

        #endregion
    }
}
