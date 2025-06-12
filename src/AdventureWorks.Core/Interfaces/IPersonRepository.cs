using AdventureWorks.Core.Entities;

namespace AdventureWorks.Core.Interfaces;

public interface IPersonRepository : IGenericRepository<Person>
{
    Task<IEnumerable<Person>> GetByFirstNameAsync(string firstName);
    Task<IEnumerable<Person>> GetByLastNameAsync(string lastName);
    Task<Person?> GetByEmailAsync(string email);
    Task<IEnumerable<Person>> SearchAsync(string searchTerm);
}
