using AdventureWorks.Core.Entities;
using AdventureWorks.Shared.DTOs;

namespace AdventureWorks.Infrastructure.Mappings
{
    /// <summary>
    /// Manual mapping methods between SalesOrderHeader entity and DTOs
    /// </summary>
    public static class SalesOrderMappings
    {
        public static SalesOrderHeaderDto ToDto(this SalesOrderHeader entity)
        {
            if (entity == null) return null!;

            return new SalesOrderHeaderDto
            {
                SalesOrderId = entity.SalesOrderId,
                RevisionNumber = entity.RevisionNumber,
                OrderDate = entity.OrderDate,
                DueDate = entity.DueDate,
                ShipDate = entity.ShipDate,
                Status = entity.Status,
                OnlineOrderFlag = entity.OnlineOrderFlag,
                SalesOrderNumber = entity.SalesOrderNumber,
                PurchaseOrderNumber = entity.PurchaseOrderNumber,
                AccountNumber = entity.AccountNumber,
                CustomerId = entity.CustomerId,
                SalesPersonId = entity.SalesPersonId,
                TerritoryId = entity.TerritoryId,
                BillToAddressId = entity.BillToAddressId,
                ShipToAddressId = entity.ShipToAddressId,
                ShipMethodId = entity.ShipMethodId,
                CreditCardId = entity.CreditCardId,
                CreditCardApprovalCode = entity.CreditCardApprovalCode,
                CurrencyRateId = entity.CurrencyRateId,
                SubTotal = entity.SubTotal,
                TaxAmt = entity.TaxAmt,
                Freight = entity.Freight,
                TotalDue = entity.TotalDue,
                Comment = entity.Comment,
                Rowguid = entity.Rowguid,
                ModifiedDate = entity.ModifiedDate,
                // Navigation properties
                Customer = entity.Customer?.ToDto(),
                BillToAddress = entity.BillToAddress?.ToDto(),
                ShipToAddress = entity.ShipToAddress?.ToDto(),
                SalesPersonName = $"{entity.SalesPerson?.BusinessEntity?.BusinessEntity?.FirstName} {entity.SalesPerson?.BusinessEntity?.BusinessEntity?.LastName}".Trim(),
                TerritoryName = entity.Territory?.Name,
                ShipMethodName = entity.ShipMethod?.Name,
                CreditCardType = entity.CreditCard?.CardType,
                OrderDetails = entity.SalesOrderDetails?.Select(d => d.ToDto()).ToList() ?? new List<SalesOrderDetailDto>()
            };
        }

        public static SalesOrderHeader ToEntity(this SalesOrderHeaderDto dto)
        {
            if (dto == null) return null!;

            return new SalesOrderHeader
            {
                SalesOrderId = dto.SalesOrderId,
                RevisionNumber = dto.RevisionNumber,
                OrderDate = dto.OrderDate,
                DueDate = dto.DueDate,
                ShipDate = dto.ShipDate,
                Status = dto.Status,
                OnlineOrderFlag = dto.OnlineOrderFlag,
                SalesOrderNumber = dto.SalesOrderNumber,
                PurchaseOrderNumber = dto.PurchaseOrderNumber,
                AccountNumber = dto.AccountNumber,
                CustomerId = dto.CustomerId,
                SalesPersonId = dto.SalesPersonId,
                TerritoryId = dto.TerritoryId,
                BillToAddressId = dto.BillToAddressId,
                ShipToAddressId = dto.ShipToAddressId,
                ShipMethodId = dto.ShipMethodId,
                CreditCardId = dto.CreditCardId,
                CreditCardApprovalCode = dto.CreditCardApprovalCode,
                CurrencyRateId = dto.CurrencyRateId,
                SubTotal = dto.SubTotal,
                TaxAmt = dto.TaxAmt,
                Freight = dto.Freight,
                TotalDue = dto.TotalDue,
                Comment = dto.Comment,
                Rowguid = dto.Rowguid,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public static List<SalesOrderHeaderDto> ToDto(this IEnumerable<SalesOrderHeader> entities)
        {
            return entities?.Select(ToDto).ToList() ?? new List<SalesOrderHeaderDto>();
        }
    }

    /// <summary>
    /// Manual mapping methods between SalesOrderDetail entity and DTOs
    /// </summary>
    public static class SalesOrderDetailMappings
    {
        public static SalesOrderDetailDto ToDto(this SalesOrderDetail entity)
        {
            if (entity == null) return null!;

            return new SalesOrderDetailDto
            {
                SalesOrderId = entity.SalesOrderId,
                SalesOrderDetailId = entity.SalesOrderDetailId,
                CarrierTrackingNumber = entity.CarrierTrackingNumber,
                OrderQty = entity.OrderQty,
                ProductId = entity.ProductId,
                SpecialOfferId = entity.SpecialOfferId,
                UnitPrice = entity.UnitPrice,
                UnitPriceDiscount = entity.UnitPriceDiscount,
                LineTotal = entity.LineTotal,
                Rowguid = entity.Rowguid,
                ModifiedDate = entity.ModifiedDate,
                // Navigation properties
                Product = entity.SpecialOfferProduct?.Product?.ToDto(),
                ProductName = entity.SpecialOfferProduct?.Product?.Name,
                ProductNumber = entity.SpecialOfferProduct?.Product?.ProductNumber,
                SpecialOfferDescription = entity.SpecialOfferProduct?.SpecialOffer?.Description
            };
        }

        public static SalesOrderDetail ToEntity(this SalesOrderDetailDto dto)
        {
            if (dto == null) return null!;

            return new SalesOrderDetail
            {
                SalesOrderId = dto.SalesOrderId,
                SalesOrderDetailId = dto.SalesOrderDetailId,
                CarrierTrackingNumber = dto.CarrierTrackingNumber,
                OrderQty = dto.OrderQty,
                ProductId = dto.ProductId,
                SpecialOfferId = dto.SpecialOfferId,
                UnitPrice = dto.UnitPrice,
                UnitPriceDiscount = dto.UnitPriceDiscount,
                LineTotal = dto.LineTotal,
                Rowguid = dto.Rowguid,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public static List<SalesOrderDetailDto> ToDto(this IEnumerable<SalesOrderDetail> entities)
        {
            return entities?.Select(ToDto).ToList() ?? new List<SalesOrderDetailDto>();
        }
    }
}
