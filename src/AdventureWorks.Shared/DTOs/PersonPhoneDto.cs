using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object for PersonPhone entity
    /// </summary>
    public class PersonPhoneDto
    {
        [Display(Name = "Business Entity ID")]
        public int BusinessEntityId { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Phone Number")]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Display(Name = "Phone Type")]
        public int PhoneNumberTypeId { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        [Display(Name = "Phone Type")]
        public string? PhoneNumberTypeName { get; set; }

        [Display(Name = "Person Name")]
        public string? PersonName { get; set; }

        // Display properties
        [Display(Name = "Formatted Phone")]
        public string FormattedPhone => $"{PhoneNumber} ({PhoneNumberTypeName})";
    }
}
