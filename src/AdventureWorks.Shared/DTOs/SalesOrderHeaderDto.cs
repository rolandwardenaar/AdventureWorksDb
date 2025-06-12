using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Shared.DTOs
{
    /// <summary>
    /// Data Transfer Object for SalesOrderHeader entity
    /// </summary>
    public class SalesOrderHeaderDto
    {
        [Display(Name = "Order ID")]
        public int SalesOrderId { get; set; }

        [Display(Name = "Revision")]
        public byte RevisionNumber { get; set; }

        [Required]
        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; }

        [Required]
        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Display(Name = "Ship Date")]
        [DataType(DataType.Date)]
        public DateTime? ShipDate { get; set; }

        [Display(Name = "Status")]
        public byte Status { get; set; }

        [Display(Name = "Online Order")]
        public bool OnlineOrderFlag { get; set; }

        [Required]
        [StringLength(25)]
        [Display(Name = "Order Number")]
        public string SalesOrderNumber { get; set; } = null!;

        [StringLength(25)]
        [Display(Name = "PO Number")]
        public string? PurchaseOrderNumber { get; set; }

        [StringLength(15)]
        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Display(Name = "Sales Person")]
        public int? SalesPersonId { get; set; }

        [Display(Name = "Territory")]
        public int? TerritoryId { get; set; }

        [Required]
        [Display(Name = "Bill To Address")]
        public int BillToAddressId { get; set; }

        [Required]
        [Display(Name = "Ship To Address")]
        public int ShipToAddressId { get; set; }

        [Required]
        [Display(Name = "Ship Method")]
        public int ShipMethodId { get; set; }

        [Display(Name = "Credit Card")]
        public int? CreditCardId { get; set; }

        [StringLength(15)]
        [Display(Name = "CC Approval Code")]
        public string? CreditCardApprovalCode { get; set; }

        [Display(Name = "Currency Rate")]
        public int? CurrencyRateId { get; set; }

        [Required]
        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal SubTotal { get; set; }

        [Required]
        [Display(Name = "Tax Amount")]
        [DataType(DataType.Currency)]
        public decimal TaxAmt { get; set; }

        [Required]
        [Display(Name = "Freight")]
        [DataType(DataType.Currency)]
        public decimal Freight { get; set; }

        [Required]
        [Display(Name = "Total Due")]
        [DataType(DataType.Currency)]
        public decimal TotalDue { get; set; }

        [StringLength(128)]
        [Display(Name = "Comment")]
        public string? Comment { get; set; }

        public Guid Rowguid { get; set; }

        [Display(Name = "Modified Date")]
        public DateTime ModifiedDate { get; set; }

        // Navigation properties
        public CustomerDto? Customer { get; set; }
        public AddressDto? BillToAddress { get; set; }
        public AddressDto? ShipToAddress { get; set; }

        [Display(Name = "Sales Person")]
        public string? SalesPersonName { get; set; }

        [Display(Name = "Territory")]
        public string? TerritoryName { get; set; }

        [Display(Name = "Ship Method")]
        public string? ShipMethodName { get; set; }

        [Display(Name = "Credit Card")]
        public string? CreditCardType { get; set; }

        // Order details
        public List<SalesOrderDetailDto> OrderDetails { get; set; } = new List<SalesOrderDetailDto>();

        // Display properties
        [Display(Name = "Status")]
        public string StatusDisplay => Status switch
        {
            1 => "In Process",
            2 => "Approved",
            3 => "Backordered",
            4 => "Rejected",
            5 => "Shipped",
            6 => "Cancelled",
            _ => "Unknown"
        };

        [Display(Name = "Order Type")]
        public string OrderType => OnlineOrderFlag ? "Online" : "Sales Person";

        [Display(Name = "Days Since Order")]
        public int DaysSinceOrder => (DateTime.Now - OrderDate).Days;

        [Display(Name = "Is Overdue")]
        public bool IsOverdue => !ShipDate.HasValue && DateTime.Now > DueDate;
    }
}
