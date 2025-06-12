using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Core.Interfaces
{
    /// <summary>
    /// Service interface for Person business operations
    /// </summary>
    public interface IPersonService
    {
        // Read operations
        Task<IEnumerable<PersonListDto>> GetAllPersonsAsync();
        Task<PagedResult<PersonListDto>> GetPersonsPagedAsync(PersonQueryParameters queryParams);
        Task<PersonDto?> GetPersonByIdAsync(int businessEntityId);
        Task<IEnumerable<PersonListDto>> SearchPersonsAsync(string searchTerm);
        Task<IEnumerable<PersonPhoneDto>> GetPersonPhonesAsync(int businessEntityId);
        Task<IEnumerable<EmailAddressDto>> GetPersonEmailsAsync(int businessEntityId);
        Task<IEnumerable<BusinessEntityAddressDto>> GetPersonAddressesAsync(int businessEntityId);

        // Write operations
        Task<PersonDto> CreatePersonAsync(PersonCreateDto personDto);
        Task<PersonDto> UpdatePersonAsync(PersonUpdateDto personDto);
        Task<bool> DeletePersonAsync(int businessEntityId);

        // Contact information operations
        Task<PersonPhoneDto> AddPhoneAsync(PersonPhoneDto phoneDto);
        Task<EmailAddressDto> AddEmailAsync(EmailAddressDto emailDto);
        Task<BusinessEntityAddressDto> AddAddressAsync(BusinessEntityAddressDto addressDto);
        Task<bool> RemovePhoneAsync(int businessEntityId, int phoneNumberTypeId);
        Task<bool> RemoveEmailAsync(int businessEntityId, int emailAddressId);
        Task<bool> RemoveAddressAsync(int businessEntityId, int addressId);

        // Business logic operations
        Task<bool> PersonExistsAsync(int businessEntityId);
        Task<bool> IsEmailUniqueAsync(string email, int? excludeBusinessEntityId = null);
        Task<IEnumerable<PersonListDto>> GetPersonsByTypeAsync(string personType);
    }
}
