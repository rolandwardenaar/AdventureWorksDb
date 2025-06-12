using AdventureWorks.Core.Entities;

namespace AdventureWorks.Core.Interfaces;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    Task<IEnumerable<Customer>> GetByTerritoryAsync(int territoryId);
    Task<Customer?> GetByAccountNumberAsync(string accountNumber);
    Task<IEnumerable<Customer>> GetIndividualCustomersAsync();
    Task<IEnumerable<Customer>> GetStoreCustomersAsync();
    
    // Additional methods needed for CustomerService
    Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm);
    Task<Customer?> GetLastCustomerAsync();
    Task<IEnumerable<Customer>> GetTopCustomersAsync(int count);
}
