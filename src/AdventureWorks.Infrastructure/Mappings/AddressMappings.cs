using AdventureWorks.Core.Entities;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Infrastructure.Mappings
{
    /// <summary>
    /// Manual mapping methods between Address entity and DTOs
    /// </summary>
    public static class AddressMappings
    {
        public static AddressDto ToDto(this Address entity)
        {
            if (entity == null) return null!;

            return new AddressDto
            {
                AddressId = entity.AddressId,
                AddressLine1 = entity.AddressLine1,
                AddressLine2 = entity.AddressLine2,
                City = entity.City,
                StateProvinceId = entity.StateProvinceId,
                PostalCode = entity.PostalCode,
                Rowguid = entity.Rowguid,
                ModifiedDate = entity.ModifiedDate,
                // Navigation properties
                StateProvinceName = entity.StateProvince?.Name,
                CountryName = entity.StateProvince?.CountryRegionCodeNavigation?.Name
            };
        }

        public static Address ToEntity(this AddressDto dto)
        {
            if (dto == null) return null!;

            return new Address
            {
                AddressId = dto.AddressId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                StateProvinceId = dto.StateProvinceId,
                PostalCode = dto.PostalCode,
                Rowguid = dto.Rowguid,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public static List<AddressDto> ToDto(this IEnumerable<Address> entities)
        {
            return entities?.Select(ToDto).ToList() ?? new List<AddressDto>();
        }
    }

    /// <summary>
    /// Manual mapping methods between PersonPhone entity and DTOs
    /// </summary>
    public static class PersonPhoneMappings
    {
        public static PersonPhoneDto ToDto(this PersonPhone entity)
        {
            if (entity == null) return null!;

            return new PersonPhoneDto
            {
                BusinessEntityId = entity.BusinessEntityId,
                PhoneNumber = entity.PhoneNumber,
                PhoneNumberTypeId = entity.PhoneNumberTypeId,
                ModifiedDate = entity.ModifiedDate,
                // Navigation properties
                PhoneNumberTypeName = entity.PhoneNumberType?.Name,
                PersonName = $"{entity.BusinessEntity?.FirstName} {entity.BusinessEntity?.LastName}".Trim()
            };
        }

        public static PersonPhone ToEntity(this PersonPhoneDto dto)
        {
            if (dto == null) return null!;

            return new PersonPhone
            {
                BusinessEntityId = dto.BusinessEntityId,
                PhoneNumber = dto.PhoneNumber,
                PhoneNumberTypeId = dto.PhoneNumberTypeId,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public static List<PersonPhoneDto> ToDto(this IEnumerable<PersonPhone> entities)
        {
            return entities?.Select(ToDto).ToList() ?? new List<PersonPhoneDto>();
        }
    }

    /// <summary>
    /// Manual mapping methods between EmailAddress entity and DTOs
    /// </summary>
    public static class EmailAddressMappings
    {
        public static EmailAddressDto ToDto(this EmailAddress entity)
        {
            if (entity == null) return null!;

            return new EmailAddressDto
            {
                BusinessEntityId = entity.BusinessEntityId,
                EmailAddressId = entity.EmailAddressId,
                EmailAddress1 = entity.EmailAddress1,
                Rowguid = entity.Rowguid,
                ModifiedDate = entity.ModifiedDate,
                // Navigation properties
                PersonName = $"{entity.BusinessEntity?.FirstName} {entity.BusinessEntity?.LastName}".Trim()
            };
        }

        public static EmailAddress ToEntity(this EmailAddressDto dto)
        {
            if (dto == null) return null!;

            return new EmailAddress
            {
                BusinessEntityId = dto.BusinessEntityId,
                EmailAddressId = dto.EmailAddressId,
                EmailAddress1 = dto.EmailAddress1,
                Rowguid = dto.Rowguid,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public static List<EmailAddressDto> ToDto(this IEnumerable<EmailAddress> entities)
        {
            return entities?.Select(ToDto).ToList() ?? new List<EmailAddressDto>();
        }
    }
}
