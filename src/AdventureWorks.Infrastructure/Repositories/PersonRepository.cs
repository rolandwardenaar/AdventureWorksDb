using Microsoft.EntityFrameworkCore;
using AdventureWorks.Core.Interfaces;
using AdventureWorks.Infrastructure.Data;
using AdventureWorks.Core.Entities;

namespace AdventureWorks.Infrastructure.Repositories;

public class PersonRepository : GenericRepository<Person>, IPersonRepository
{
    public PersonRepository(AdventureWorksDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Person>> GetByFirstNameAsync(string firstName)
    {
        return await _dbSet
            .Where(p => p.FirstName.Contains(firstName))
            .OrderBy(p => p.LastName)
            .ToListAsync();
    }

    public async Task<IEnumerable<Person>> GetByLastNameAsync(string lastName)
    {
        return await _dbSet
            .Where(p => p.LastName.Contains(lastName))
            .OrderBy(p => p.FirstName)
            .ToListAsync();
    }

    public async Task<Person?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .Include(p => p.EmailAddresses)
            .FirstOrDefaultAsync(p => p.EmailAddresses.Any(e => e.EmailAddress1 == email));
    }

    public async Task<IEnumerable<Person>> SearchAsync(string searchTerm)
    {
        return await _dbSet
            .Where(p => p.FirstName.Contains(searchTerm) || 
                       p.LastName.Contains(searchTerm) ||
                       p.MiddleName != null && p.MiddleName.Contains(searchTerm))
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }    public override async Task<Person?> GetByIdAsync(int id)
    {
        return await _dbSet
            .Include(p => p.EmailAddresses)
            .Include(p => p.PersonPhones)
            .Include(p => p.BusinessEntity)
                .ThenInclude(be => be.BusinessEntityAddresses)
                    .ThenInclude(bea => bea.Address)
            .FirstOrDefaultAsync(p => p.BusinessEntityId == id);
    }
}
