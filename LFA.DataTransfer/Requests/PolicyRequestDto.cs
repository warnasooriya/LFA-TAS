using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class PolicyRequestDto
    {
        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid ProductId { get; set; }
        public Guid DealerId { get; set; }
        public Guid DealerLocationId { get; set; }
        public Guid ContractId { get; set; }
        public Guid ExtensionTypeId { get; set; }
        public Decimal Premium { get; set; }
        public Guid PremiumCurrencyTypeId { get; set; }
        public Guid TPABranchId { get; set; }
        public Guid CurrencyPeriodId { get; set; }
        public Guid CoverTypeId { get; set; }
        public string HrsUsedAtPolicySale { get; set; }
        public bool IsPreWarrantyCheck { get; set; }
        public DateTime PolicySoldDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public Guid SalesPersonId { get; set; }
        public string PolicyNo { get; set; }
        public bool IsSpecialDeal { get; set; }
        public bool IsPartialPayment { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovedDate { get; set; }
        public Decimal DealerPayment { get; set; }
        public Decimal Discount { get; set; }
        public int DiscountPercentage { get; set; }
        public Guid DealerPaymentCurrencyTypeId { get; set; }
        public Decimal CustomerPayment { get; set; }
        public Guid CustomerPaymentCurrencyTypeId { get; set; }
        public Guid PaymentModeId { get; set; }
        public Guid PaymentTypeId { get; set; }

        public string RefNo { get; set; }
        public string Comment { get; set; }
        public Guid ItemId { get; set; }
        public string Type { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public bool IsPolicyCanceled { get; set; }
        public Guid PolicyBundleId { get; set; }
        public Decimal TransferFee { get; set; }
        public Guid BordxId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string ForwardComment { get; set; }
        public bool DealerPolicy { get; set; }
        public decimal LocalCurrencyConversionRate { get; set; }


        public Decimal PaymentMethodFee { get; set; }
        public Decimal PaymentMethodFeePercentage { get; set; }
        public Decimal GrossPremiumBeforeTax { get; set; }
        public Decimal EligibilityFee { get; set; }
        public Decimal NRP { get; set; }
        public Decimal TotalTax { get; set; }

        public Guid Co_Customer { get; set; }

        public  string BookletNumber { get; set; }

        public  Guid ContractInsuaranceLimitationId { get; set; }
        public  Guid ContractExtensionsId { get; set; }
        public  Guid ContractExtensionPremiumId { get; set; }
        public DateTime MWStartDate { get; set; }
        public bool MWIsAvailable { get; set; }
        public Guid AttributeSpecificationId { get; set; }
        public Decimal EmiValue { get; set; }
        public List<PolicyContractProductRequestDto> ContractProducts { get; set; }



        public bool PolicyInsertion
        {
            get;
            set;
        }



    }
}
