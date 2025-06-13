using AdventureWorks.Core.Interfaces;
using AdventureWorks.Infrastructure.Mappings;
using AdventureWorks.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AdventureWorks.Infrastructure.Services
{
    /// <summary>
    /// Service implementation for Person business operations
    /// </summary>
    public class PersonService : IPersonService
    {
        private readonly IPersonRepository _personRepository;
        private readonly IGenericRepository<Core.Entities.PersonPhone> _phoneRepository;
        private readonly IGenericRepository<Core.Entities.EmailAddress> _emailRepository;
        private readonly IGenericRepository<Core.Entities.BusinessEntityAddress> _addressRepository;

        public PersonService(
            IPersonRepository personRepository,
            IGenericRepository<Core.Entities.PersonPhone> phoneRepository,
            IGenericRepository<Core.Entities.EmailAddress> emailRepository,
            IGenericRepository<Core.Entities.BusinessEntityAddress> addressRepository)
        {
            _personRepository = personRepository;
            _phoneRepository = phoneRepository;
            _emailRepository = emailRepository;
            _addressRepository = addressRepository;
        }

        #region Read Operations

        public async Task<IEnumerable<PersonListDto>> GetAllPersonsAsync()
        {
            var persons = await _personRepository.GetAllAsync();
            return persons.ToListDto();
        }        public async Task<PersonDetailDto?> GetPersonByIdAsync(int businessEntityId)
        {
            var person = await _personRepository.GetByIdWithDetailsAsync(businessEntityId);
            return person?.ToDetailDto();
        }

        public async Task<IEnumerable<PersonListDto>> SearchPersonsAsync(string searchTerm)
        {
            var persons = await _personRepository.SearchPersonsAsync(searchTerm);
            return persons.ToListDto();
        }

        public async Task<IEnumerable<PersonPhoneDto>> GetPersonPhonesAsync(int businessEntityId)
        {
            var phones = await _phoneRepository.FindAsync(p => p.BusinessEntityId == businessEntityId);
            return phones.ToDto();
        }

        public async Task<IEnumerable<EmailAddressDto>> GetPersonEmailsAsync(int businessEntityId)
        {
            var emails = await _emailRepository.FindAsync(e => e.BusinessEntityId == businessEntityId);
            return emails.ToDto();
        }

        public async Task<IEnumerable<BusinessEntityAddressDto>> GetPersonAddressesAsync(int businessEntityId)
        {
            var addresses = await _addressRepository.FindAsync(a => a.BusinessEntityId == businessEntityId);
            return addresses.Select(a => new BusinessEntityAddressDto
            {
                BusinessEntityId = a.BusinessEntityId,
                AddressId = a.AddressId,
                AddressTypeId = a.AddressTypeId,
                Rowguid = a.Rowguid,
                ModifiedDate = a.ModifiedDate,
                Address = a.Address?.ToDto(),
                AddressTypeName = a.AddressType?.Name,
                BusinessEntityName = $"Person {businessEntityId}"
            });
        }

        public async Task<IEnumerable<PersonListDto>> GetPersonsByTypeAsync(string personType)
        {
            var persons = await _personRepository.GetPersonsByTypeAsync(personType);
            return persons.ToListDto();
        }

        public async Task<PagedResult<PersonListDto>> GetPersonsPagedAsync(PersonQueryParameters queryParams)
        {
            // Build the query
            var query = _personRepository.GetQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(queryParams.PersonType))
            {
                query = query.Where(p => p.PersonType == queryParams.PersonType);
            }

            if (!string.IsNullOrWhiteSpace(queryParams.FirstName))
            {
                query = query.Where(p => p.FirstName.Contains(queryParams.FirstName));
            }

            if (!string.IsNullOrWhiteSpace(queryParams.LastName))
            {
                query = query.Where(p => p.LastName.Contains(queryParams.LastName));
            }

            if (!string.IsNullOrWhiteSpace(queryParams.SearchTerm))
            {
                var searchTerm = queryParams.SearchTerm.ToLower();
                query = query.Where(p => 
                    p.FirstName.ToLower().Contains(searchTerm) ||
                    p.LastName.ToLower().Contains(searchTerm) ||
                    (p.MiddleName != null && p.MiddleName.ToLower().Contains(searchTerm)) ||
                    p.EmailAddresses.Any(e => e.EmailAddress1.ToLower().Contains(searchTerm)));
            }

            if (queryParams.CreatedAfter.HasValue)
            {
                query = query.Where(p => p.ModifiedDate >= queryParams.CreatedAfter.Value);
            }

            if (queryParams.CreatedBefore.HasValue)
            {
                query = query.Where(p => p.ModifiedDate <= queryParams.CreatedBefore.Value);
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(queryParams.SortBy))
            {
                query = queryParams.SortBy.ToLower() switch
                {
                    "firstname" => queryParams.SortDescending 
                        ? query.OrderByDescending(p => p.FirstName)
                        : query.OrderBy(p => p.FirstName),
                    "lastname" => queryParams.SortDescending 
                        ? query.OrderByDescending(p => p.LastName)
                        : query.OrderBy(p => p.LastName),
                    "persontype" => queryParams.SortDescending 
                        ? query.OrderByDescending(p => p.PersonType)
                        : query.OrderBy(p => p.PersonType),
                    "modifieddate" => queryParams.SortDescending 
                        ? query.OrderByDescending(p => p.ModifiedDate)
                        : query.OrderBy(p => p.ModifiedDate),
                    _ => queryParams.SortDescending 
                        ? query.OrderByDescending(p => p.BusinessEntityId)
                        : query.OrderBy(p => p.BusinessEntityId)
                };
            }
            else
            {
                query = query.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
            }

            // Get total count before paging
            var totalCount = await query.CountAsync();

            // Apply paging
            var items = await query
                .Skip((queryParams.Page - 1) * queryParams.PageSize)
                .Take(queryParams.PageSize)
                .ToListAsync();

            // Map to DTOs
            var dtoItems = items.ToListDto();

            return new PagedResult<PersonListDto>
            {
                Items = dtoItems,
                TotalCount = totalCount,
                Page = queryParams.Page,
                PageSize = queryParams.PageSize
            };
        }

        #endregion

        #region Write Operations

        public async Task<PersonDetailDto> CreatePersonAsync(PersonCreateDto personDto)
        {
            var person = personDto.ToEntity();
            var createdPerson = await _personRepository.AddAsync(person);
            return createdPerson.ToDetailDto();
        }

        public async Task<PersonDetailDto> UpdatePersonAsync(int businessEntityId, PersonUpdateDto personDto)
        {
            var existingPerson = await _personRepository.GetByIdAsync(businessEntityId);
            if (existingPerson == null)
                throw new ArgumentException($"Person with ID {businessEntityId} not found.");

            personDto.UpdateEntity(existingPerson);
            var updatedPerson = await _personRepository.UpdateAsync(existingPerson);
            return updatedPerson.ToDetailDto();
        }

        public async Task<bool> DeletePersonAsync(int businessEntityId)
        {
            var person = await _personRepository.GetByIdAsync(businessEntityId);
            if (person == null)
                return false;

            return await _personRepository.DeleteAsync(person);
        }

        #endregion

        #region Contact Information Operations

        public async Task<PersonPhoneDto> AddPhoneAsync(PersonPhoneDto phoneDto)
        {
            var phone = phoneDto.ToEntity();
            var addedPhone = await _phoneRepository.AddAsync(phone);
            return addedPhone.ToDto();
        }

        public async Task<EmailAddressDto> AddEmailAsync(EmailAddressDto emailDto)
        {
            var email = emailDto.ToEntity();
            var addedEmail = await _emailRepository.AddAsync(email);
            return addedEmail.ToDto();
        }

        public async Task<BusinessEntityAddressDto> AddAddressAsync(BusinessEntityAddressDto addressDto)
        {
            var address = new Core.Entities.BusinessEntityAddress
            {
                BusinessEntityId = addressDto.BusinessEntityId,
                AddressId = addressDto.AddressId,
                AddressTypeId = addressDto.AddressTypeId,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.UtcNow
            };
            
            var addedAddress = await _addressRepository.AddAsync(address);
            return new BusinessEntityAddressDto
            {
                BusinessEntityId = addedAddress.BusinessEntityId,
                AddressId = addedAddress.AddressId,
                AddressTypeId = addedAddress.AddressTypeId,
                Rowguid = addedAddress.Rowguid,
                ModifiedDate = addedAddress.ModifiedDate
            };
        }

        public async Task<bool> RemovePhoneAsync(int businessEntityId, int phoneNumberTypeId)
        {
            var phone = await _phoneRepository.FindFirstAsync(p => 
                p.BusinessEntityId == businessEntityId && p.PhoneNumberTypeId == phoneNumberTypeId);
            
            if (phone == null)
                return false;

            return await _phoneRepository.DeleteAsync(phone);
        }

        public async Task<bool> RemoveEmailAsync(int businessEntityId, int emailAddressId)
        {
            var email = await _emailRepository.FindFirstAsync(e => 
                e.BusinessEntityId == businessEntityId && e.EmailAddressId == emailAddressId);
            
            if (email == null)
                return false;

            return await _emailRepository.DeleteAsync(email);
        }

        public async Task<bool> RemoveAddressAsync(int businessEntityId, int addressId)
        {
            var address = await _addressRepository.FindFirstAsync(a => 
                a.BusinessEntityId == businessEntityId && a.AddressId == addressId);
            
            if (address == null)
                return false;

            return await _addressRepository.DeleteAsync(address);
        }

        #endregion

        #region Business Logic Operations

        public async Task<bool> PersonExistsAsync(int businessEntityId)
        {
            var person = await _personRepository.GetByIdAsync(businessEntityId);
            return person != null;
        }

        public async Task<bool> IsEmailUniqueAsync(string email, int? excludeBusinessEntityId = null)
        {
            var existingEmail = await _emailRepository.FindFirstAsync(e => 
                e.EmailAddress1 == email && 
                (excludeBusinessEntityId == null || e.BusinessEntityId != excludeBusinessEntityId));
            
            return existingEmail == null;
        }

        #endregion
    }
}
