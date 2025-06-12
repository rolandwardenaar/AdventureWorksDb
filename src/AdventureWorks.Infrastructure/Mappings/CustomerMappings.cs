using AdventureWorks.Core.Entities;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Infrastructure.Mappings
{
    /// <summary>
    /// Manual mapping methods between Customer entity and DTOs
    /// </summary>
    public static class CustomerMappings
    {
        public static CustomerDto ToDto(this Customer entity)
        {
            if (entity == null) return null!;

            return new CustomerDto
            {
                CustomerId = entity.CustomerId,
                PersonId = entity.PersonId,
                StoreId = entity.StoreId,
                TerritoryId = entity.TerritoryId,
                AccountNumber = entity.AccountNumber,
                Rowguid = entity.Rowguid,
                ModifiedDate = entity.ModifiedDate,
                // Navigation properties
                Person = entity.Person?.ToDto(),
                StoreName = entity.Store?.Name,
                TerritoryName = entity.Territory?.Name
            };
        }

        public static Customer ToEntity(this CustomerDto dto)
        {
            if (dto == null) return null!;

            return new Customer
            {
                CustomerId = dto.CustomerId,
                PersonId = dto.PersonId,
                StoreId = dto.StoreId,
                TerritoryId = dto.TerritoryId,
                AccountNumber = dto.AccountNumber,
                Rowguid = dto.Rowguid,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public static List<CustomerDto> ToDto(this IEnumerable<Customer> entities)
        {
            return entities?.Select(ToDto).ToList() ?? new List<CustomerDto>();
        }
    }
}
