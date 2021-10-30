using System;

namespace TAS.Services.Entities
{
    public class Policy
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid DealerLocationId { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Guid ExtensionTypeId { get; set; }
        public virtual Decimal Premium { get; set; }
        public virtual Guid TPABranchId { get; set; }
        public virtual Guid PremiumCurrencyTypeId { get; set; }
        public virtual Guid CoverTypeId { get; set; }
        public virtual string HrsUsedAtPolicySale { get; set; }
        public virtual bool IsPreWarrantyCheck { get; set; }
        public virtual DateTime PolicySoldDate { get; set; }
        public virtual DateTime PolicyStartDate { get; set; }
        public virtual DateTime PolicyEndDate { get; set; }
        public virtual Guid SalesPersonId { get; set; }
        public virtual string PolicyNo { get; set; }
        public virtual bool IsSpecialDeal { get; set; }
        public virtual bool IsPartialPayment { get; set; }
        public virtual Decimal DealerPayment { get; set; }
        public virtual Decimal Discount { get; set; }
        public virtual Guid DealerPaymentCurrencyTypeId { get; set; }
        public virtual Decimal CustomerPayment { get; set; }
        public virtual Guid CustomerPaymentCurrencyTypeId { get; set; }
        public virtual Guid PaymentModeId { get; set; }
        public virtual Guid PaymentTypeId { get; set; }

        public virtual int UniqueRef { get; set; }
        public virtual string RefNo { get; set; }
        public virtual string Comment { get; set; }
        public virtual Guid CustomerId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual Guid PolicyApprovedBy { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual decimal LocalCurrencyConversionRate { get; set; }
        public virtual Guid BordxCountryId { get; set; }
        public virtual bool IsPolicyCanceled { get; set; }
        public virtual bool IsPolicyRenewed { get; set; }

        public virtual Guid PolicyBundleId { get; set; }
        public virtual Decimal TransferFee { get; set; }
        public virtual Guid BordxId { get; set; }
        public virtual int Year { get; set; }
        public virtual int Month { get; set; }
        public virtual int BordxNumber { get; set; }

        public virtual Decimal PaymentMethodFee { get; set; }
        public virtual Decimal PaymentMethodFeePercentage { get; set; }
        public virtual Decimal GrossPremiumBeforeTax { get; set; }
        public virtual Decimal EligibilityFee { get; set; }
        public virtual Decimal NRP { get; set; }
        public virtual Decimal TotalTax { get; set; }

        public virtual int SequenceNo { get; set; }

        public virtual string ForwardComment { get; set; }
        public virtual bool DealerPolicy { get; set; }



        public virtual Guid Co_Customer { get; set; }
        public virtual Guid ContractInsuaranceLimitationId { get; set; }
        public virtual Guid ContractExtensionsId { get; set; }
        public virtual Guid ContractExtensionPremiumId { get; set; }
        public virtual decimal DiscountPercentage { get; set; }
        public virtual string BookletNumber { get; set; }
        public virtual DateTime MWStartDate { get; set; }
        public virtual bool MWIsAvailable { get; set; }
        public virtual Decimal MonthlyEMI { get; set; }
        public virtual Decimal FinanceAmount { get; set; }
        public virtual DateTime ApprovedDate { get; set; }

    }
    public class PolicyContractProduct
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual Guid ParentProductId { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Guid ExtensionTypeId { get; set; }
        public virtual Decimal Premium { get; set; }
        public virtual Guid PremiumCurrencyTypeId { get; set; }
        public virtual Guid CoverTypeId { get; set; }
        public virtual string Type { get; set; }
        public virtual string PolicyNo { get; set; }
        public virtual DateTime PolicyStartDate { get; set; }
        public virtual DateTime PolicyEndDate { get; set; }
        public virtual Guid ReferenceId { get; set; }
    }
    public class BAndWPolicy
    {
        public virtual Guid Id { get; set; }
        public virtual Guid BAndWId { get; set; }
        public virtual Guid PolicyId { get; set; }
    }
    public class VehiclePolicy
    {
        public virtual Guid Id { get; set; }
        public virtual Guid VehicleId { get; set; }
        public virtual Guid PolicyId { get; set; }
    }

    public class PolicyInfo : Policy
    {
        public virtual Guid ItemId { get; set; }
        public virtual string Type { get; set; }
    }

}
