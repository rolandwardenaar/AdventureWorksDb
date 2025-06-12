using Microsoft.EntityFrameworkCore;
using AdventureWorks.Core.Interfaces;
using AdventureWorks.Infrastructure.Data;
using AdventureWorks.Core.Entities;

namespace AdventureWorks.Infrastructure.Repositories;

public class SalesOrderRepository : GenericRepository<SalesOrderHeader>, ISalesOrderRepository
{
    public SalesOrderRepository(AdventureWorksDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<SalesOrderHeader>> GetByCustomerAsync(int customerId)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c!.Person)
            .Where(so => so.CustomerId == customerId)
            .OrderByDescending(so => so.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SalesOrderHeader>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c!.Person)
            .Include(so => so.SalesPerson)
            .Where(so => so.OrderDate >= startDate && so.OrderDate <= endDate)
            .OrderByDescending(so => so.OrderDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<SalesOrderHeader>> GetByStatusAsync(int status)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c!.Person)
            .Where(so => so.Status == status)
            .OrderByDescending(so => so.OrderDate)
            .ToListAsync();
    }    public async Task<SalesOrderHeader?> GetWithDetailsAsync(int orderId)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c!.Person)
            .Include(so => so.SalesPerson)
            .Include(so => so.Territory)
            .Include(so => so.SalesOrderDetails)
                .ThenInclude(sod => sod.SpecialOfferProduct)
                    .ThenInclude(sop => sop.Product)
            .Include(so => so.BillToAddress)
            .Include(so => so.ShipToAddress)
            .FirstOrDefaultAsync(so => so.SalesOrderId == orderId);
    }

    public async Task<IEnumerable<SalesOrderHeader>> GetRecentOrdersAsync(int count)
    {
        return await _dbSet
            .Include(so => so.Customer)
                .ThenInclude(c => c!.Person)
            .Include(so => so.SalesPerson)
            .OrderByDescending(so => so.OrderDate)
            .Take(count)
            .ToListAsync();
    }

    public override async Task<SalesOrderHeader?> GetByIdAsync(int id)
    {
        return await GetWithDetailsAsync(id);
    }
}
