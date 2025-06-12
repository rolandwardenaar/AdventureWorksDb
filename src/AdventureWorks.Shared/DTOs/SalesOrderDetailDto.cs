using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object for SalesOrderDetail entity
    /// </summary>
    public class SalesOrderDetailDto
    {
        [Display(Name = "Order ID")]
        public int SalesOrderId { get; set; }

        [Display(Name = "Detail ID")]
        public int SalesOrderDetailId { get; set; }

        [StringLength(25)]
        [Display(Name = "Tracking Number")]
        public string? CarrierTrackingNumber { get; set; }

        [Required]
        [Range(1, short.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        [Display(Name = "Quantity")]
        public short OrderQty { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "Special Offer")]
        public int SpecialOfferId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Unit price must be greater than 0")]
        [Display(Name = "Unit Price")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [Range(0, 1, ErrorMessage = "Discount must be between 0 and 1")]
        [Display(Name = "Discount %")]
        public decimal UnitPriceDiscount { get; set; }

        [Display(Name = "Line Total")]
        [DataType(DataType.Currency)]
        public decimal LineTotal { get; set; }

        public Guid Rowguid { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public ProductDto? Product { get; set; }

        [Display(Name = "Product Name")]
        public string? ProductName { get; set; }

        [Display(Name = "Product Number")]
        public string? ProductNumber { get; set; }

        [Display(Name = "Special Offer Description")]
        public string? SpecialOfferDescription { get; set; }

        // Display properties
        [Display(Name = "Discount Amount")]
        [DataType(DataType.Currency)]
        public decimal DiscountAmount => UnitPrice * UnitPriceDiscount * OrderQty;

        [Display(Name = "Discount %")]
        public string DiscountPercentage => $"{UnitPriceDiscount:P}";

        [Display(Name = "Extended Price")]
        [DataType(DataType.Currency)]
        public decimal ExtendedPrice => UnitPrice * OrderQty;

        [Display(Name = "Product Summary")]
        public string ProductSummary => $"{ProductName} ({ProductNumber}) - Qty: {OrderQty}";
    }
}
