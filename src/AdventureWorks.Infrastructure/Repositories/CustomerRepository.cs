using Microsoft.EntityFrameworkCore;
using AdventureWorks.Core.Interfaces;
using AdventureWorks.Infrastructure.Data;
using AdventureWorks.Core.Entities;

namespace AdventureWorks.Infrastructure.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AdventureWorksDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Customer>> GetByTerritoryAsync(int territoryId)
    {
        return await _dbSet
            .Include(c => c.Person)
            .Include(c => c.Store)
            .Where(c => c.TerritoryId == territoryId)
            .OrderBy(c => c.AccountNumber)
            .ToListAsync();
    }

    public async Task<Customer?> GetByAccountNumberAsync(string accountNumber)
    {
        return await _dbSet
            .Include(c => c.Person)
            .Include(c => c.Store)
            .Include(c => c.Territory)
            .FirstOrDefaultAsync(c => c.AccountNumber == accountNumber);
    }

    public async Task<IEnumerable<Customer>> GetIndividualCustomersAsync()
    {
        return await _dbSet
            .Include(c => c.Person)
            .Where(c => c.PersonId != null)
            .OrderBy(c => c.Person!.LastName)
            .ThenBy(c => c.Person!.FirstName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetStoreCustomersAsync()
    {
        return await _dbSet
            .Include(c => c.Store)
            .Where(c => c.StoreId != null)
            .OrderBy(c => c.Store!.Name)
            .ToListAsync();
    }

    public override async Task<Customer?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(c => c.Person)
                .ThenInclude(p => p!.EmailAddresses)
            .Include(c => c.Person)
                .ThenInclude(p => p!.PersonPhones)
            .Include(c => c.Store)
            .Include(c => c.Territory)
            .FirstOrDefaultAsync(c => c.CustomerId == id);
    }

    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
    {
        return await _dbSet
            .Include(c => c.Person)
            .Include(c => c.Store)
            .Where(c => (c.Person != null && (c.Person.FirstName.Contains(searchTerm) || c.Person.LastName.Contains(searchTerm))) ||
                       (c.Store != null && c.Store.Name.Contains(searchTerm)) ||
                       c.AccountNumber.Contains(searchTerm))
            .OrderBy(c => c.AccountNumber)
            .ToListAsync();
    }

    public async Task<Customer?> GetLastCustomerAsync()
    {
        return await _dbSet
            .OrderByDescending(c => c.CustomerId)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Customer>> GetTopCustomersAsync(int count)
    {
        return await _dbSet
            .Include(c => c.Person)
            .Include(c => c.Store)
            .Include(c => c.SalesOrderHeaders)
            .OrderByDescending(c => c.SalesOrderHeaders.Sum(o => o.TotalDue))
            .Take(count)
            .ToListAsync();
    }
}
