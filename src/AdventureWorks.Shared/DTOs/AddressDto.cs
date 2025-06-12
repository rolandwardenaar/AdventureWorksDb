using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object for Address entity
    /// </summary>
    public class AddressDto
    {
        public int AddressId { get; set; }

        [Required]
        [StringLength(60)]
        [Display(Name = "Address Line 1")]
        public string AddressLine1 { get; set; } = null!;

        [StringLength(60)]
        [Display(Name = "Address Line 2")]
        public string? AddressLine2 { get; set; }

        [Required]
        [StringLength(30)]
        [Display(Name = "City")]
        public string City { get; set; } = null!;

        [Display(Name = "State/Province")]
        public int StateProvinceId { get; set; }

        [Required]
        [StringLength(15)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = null!;

        public Guid Rowguid { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        [Display(Name = "State/Province")]
        public string? StateProvinceName { get; set; }

        [Display(Name = "Country")]
        public string? CountryName { get; set; }

        // Display properties
        [Display(Name = "Full Address")]
        public string FullAddress
        {
            get
            {
                var address = AddressLine1;
                if (!string.IsNullOrEmpty(AddressLine2))
                    address += ", " + AddressLine2;
                address += $", {City}, {StateProvinceName} {PostalCode}";
                if (!string.IsNullOrEmpty(CountryName))
                    address += $", {CountryName}";
                return address;
            }
        }
    }
}
