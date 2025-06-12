using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object for BusinessEntityAddress entity
    /// </summary>
    public class BusinessEntityAddressDto
    {
        [Display(Name = "Business Entity ID")]
        public int BusinessEntityId { get; set; }

        [Required]
        [Display(Name = "Address")]
        public int AddressId { get; set; }

        [Required]
        [Display(Name = "Address Type")]
        public int AddressTypeId { get; set; }

        public Guid Rowguid { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public AddressDto? Address { get; set; }

        [Display(Name = "Address Type")]
        public string? AddressTypeName { get; set; }

        [Display(Name = "Entity Name")]
        public string? BusinessEntityName { get; set; }

        // Display properties
        [Display(Name = "Address Summary")]
        public string AddressSummary => $"{AddressTypeName}: {Address?.FullAddress}";
    }
}
