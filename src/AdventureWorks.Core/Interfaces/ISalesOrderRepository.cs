using AdventureWorks.Core.Entities;

namespace AdventureWorks.Core.Interfaces;

public interface ISalesOrderRepository : IGenericRepository<SalesOrderHeader>
{
    Task<IEnumerable<SalesOrderHeader>> GetByCustomerAsync(int customerId);
    Task<IEnumerable<SalesOrderHeader>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<SalesOrderHeader>> GetByStatusAsync(int status);
    Task<SalesOrderHeader?> GetWithDetailsAsync(int orderId);
    Task<IEnumerable<SalesOrderHeader>> GetRecentOrdersAsync(int count);
}
