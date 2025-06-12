using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object for Customer entity
    /// </summary>
    public class CustomerDto
    {
        public int CustomerId { get; set; }

        public int? PersonId { get; set; }

        public int? StoreId { get; set; }

        public int? TerritoryId { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; } = null!;

        public Guid Rowguid { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        // Navigation properties as DTOs
        public PersonDto? Person { get; set; }

        [Display(Name = "Store Name")]
        public string? StoreName { get; set; }

        [Display(Name = "Territory Name")]
        public string? TerritoryName { get; set; }

        // Display properties
        [Display(Name = "Customer Name")]
        public string CustomerName => Person?.FullName ?? StoreName ?? "Unknown";

        [Display(Name = "Customer Type")]
        public string CustomerType => PersonId.HasValue ? "Individual" : "Store";
    }
}
