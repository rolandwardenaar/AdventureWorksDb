using AdventureWorks.Core.Entities;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Infrastructure.Mappings
{
    /// <summary>
    /// Manual mapping methods between Person entity and DTOs
    /// </summary>
    public static class PersonMappings
    {
        public static PersonDto ToDto(this Person entity)
        {
            if (entity == null) return null!;

            return new PersonDto
            {
                BusinessEntityId = entity.BusinessEntityId,
                PersonType = entity.PersonType,
                FirstName = entity.FirstName,
                MiddleName = entity.MiddleName,
                LastName = entity.LastName,
                ModifiedDate = entity.ModifiedDate,
                // Navigation properties will be mapped separately if needed
                EmailAddresses = new List<string>(),
                Phones = new List<PersonPhoneDto>(),
                Addresses = new List<AddressDto>()
            };
        }        public static PersonListDto ToListDto(this Person entity)
        {
            if (entity == null) return null!;

            return new PersonListDto
            {
                BusinessEntityId = entity.BusinessEntityId,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PersonType = entity.PersonType,
                PrimaryEmail = entity.EmailAddresses?.FirstOrDefault()?.EmailAddress1,
                PrimaryPhone = entity.PersonPhones?.FirstOrDefault()?.PhoneNumber
            };
        }

        public static Person ToEntity(this PersonCreateDto dto)
        {
            if (dto == null) return null!;

            return new Person
            {
                PersonType = dto.PersonType,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                ModifiedDate = DateTime.UtcNow,
                Rowguid = Guid.NewGuid()
            };
        }

        public static void UpdateEntity(this PersonUpdateDto dto, Person entity)
        {
            if (dto == null || entity == null) return;

            entity.PersonType = dto.PersonType;
            entity.FirstName = dto.FirstName;
            entity.MiddleName = dto.MiddleName;
            entity.LastName = dto.LastName;
            entity.ModifiedDate = DateTime.UtcNow;
        }

        public static List<PersonDto> ToDto(this IEnumerable<Person> entities)
        {
            return entities?.Select(ToDto).ToList() ?? new List<PersonDto>();
        }

        public static List<PersonListDto> ToListDto(this IEnumerable<Person> entities)
        {
            return entities?.Select(ToListDto).ToList() ?? new List<PersonListDto>();
        }
    }
}
