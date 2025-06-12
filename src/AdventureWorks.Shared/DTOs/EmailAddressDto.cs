using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object for EmailAddress entity
    /// </summary>
    public class EmailAddressDto
    {
        [Display(Name = "Business Entity ID")]
        public int BusinessEntityId { get; set; }

        [Display(Name = "Email ID")]
        public int EmailAddressId { get; set; }

        [Required]
        [StringLength(50)]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string? EmailAddress1 { get; set; }

        public Guid Rowguid { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        [Display(Name = "Person Name")]
        public string? PersonName { get; set; }

        // Display properties
        [Display(Name = "Contact Info")]
        public string ContactInfo => $"{PersonName} <{EmailAddress1}>";
    }
}
