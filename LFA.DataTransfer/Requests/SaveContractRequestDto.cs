using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{

    public class Customer_
    {
        public Guid customerId { get; set; }
        public int customerTypeId { get; set; }
        public int usageTypeId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime? dateOfBirth { get; set; }
        public char gender { get; set; }
        public int idTypeId { get; set; }
        public string idNo { get; set; }
        public DateTime? idIssueDate { get; set; }
        public int nationalityId { get; set; }
        public Guid countryId { get; set; }
        public Guid cityId { get; set; }
        public string PostalCode { get; set; }
        public string mobileNo { get; set; }
        public string otherTelNo { get; set; }
        public string email { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string address3 { get; set; }
        public string address4 { get; set; }
        public string businessName { get; set; }
        public string businessTelNo { get; set; }
        public string businessAddress1 { get; set; }
        public string businessAddress2 { get; set; }
        public string businessAddress3 { get; set; }
        public string businessAddress4 { get; set; }
    }

    public class Product_
    {
        public Guid id { get; set; }
        public Guid commodityTypeId { get; set; }
        public Guid productId { get; set; }
        public Guid dealerId { get; set; }
        public Guid dealerLocationId { get; set; }
        public Guid categoryId { get; set; }
        public string serialNumber { get; set; }
        public Guid makeId { get; set; }
        public Guid modelId { get; set; }
        public string modelYear { get; set; }
        public string invoiceNo { get; set; }
        public string additionalSerial { get; set; }
        public Guid itemStatusId { get; set; }
        public Guid commodityUsageTypeId { get; set; }
        public DateTime itemPurchasedDate { get; set; }
        public decimal dealerPrice { get; set; }
        public decimal itemPrice { get; set; }
        public Guid variantId { get; set; }
        public Guid engineCapacityId { get; set; }
        public Guid cylinderCountId { get; set; }
        public Guid fuelTypeId { get; set; }
        public Guid transmissionTypeId { get; set; }
        public Guid bodyTypeId { get; set; }
        public Guid driveTypeId { get; set; }
        public Guid aspirationTypeId { get; set; }
        public Guid dealerPaymentCurrencyTypeId { get; set; }
        public Guid customerPaymentCurrencyTypeId { get; set; }
        public DateTime registrationDate { get; set; }
        public decimal grossWeight { get; set; }
        public DateTime MWStartDate { get; set; }
        public bool MWIsAvailable { get; set; }
        public string engineNumber { get; set; }

    }

    public class Contract_
    {
        public Guid CountryId { get; set; }
        public Guid DealerId { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid CommodityCategoryId { get; set; }
        public Guid ProductId { get; set; }
        public Guid LinkDealId { get; set; }
        public string DealName { get; set; }
        public Guid DealType { get; set; }
        public Guid ItemStatusId { get; set; }
        public Guid InsurerId { get; set; }
        public Guid ReinsurerId { get; set; }
        public bool IsAutoRenewal { get; set; }
        public bool IsPromotional { get; set; }
        public bool DiscountAvailable { get; set; }
        public bool IsActive { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Remark { get; set; }
        public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public decimal GrossPremium { get; set; }
        public decimal ClaimLimitation { get; set; }
        public decimal LiabilityLimitation { get; set; }
        public decimal PremiumTotal { get; set; }
        public Guid CommodityUsageTypeId { get; set; }
        public Guid Id { get; set; }
    }

    public class ExtensionType_
    {
        public Guid Id { get; set; }
        public string ExtensionName { get; set; }
        public int Km { get; set; }
        public int Month { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid ProductId { get; set; }
        public int Hours { get; set; }
        public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public bool IsExtensionTypeExists { get; set; }
    }

    public class CoverType_
    {
        public Guid Id { get; set; }
        public string WarrantyTypeDescription { get; set; }
        public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public bool IsWarrantyTypeExists { get; set; }
    }

    public class ProductContract_
    {
        public Guid ProductId { get; set; }
        public Guid ParentProductId { get; set; }
        public Guid ContractId { get; set; }
        public Guid ExtensionTypeId { get; set; }
        public Guid AttributeSpecificationId { get; set; }

        public Guid CoverTypeId { get; set; }
        public Guid Id { get; set; }

        public string PolicyNo { get; set; }
        public List<Contract_> Contracts { get; set; }
        public List<ExtensionType_> ExtensionTypes { get; set; }
        public List<CoverType_> CoverTypes { get; set; }
        public decimal Premium { get; set; }
        public string PremiumCurrencyName { get; set; }
        public Guid PremiumCurrencyTypeId { get; set; }
        public string Name { get; set; }
        public bool RSA { get; set; }

        public string BookletNumber { get; set; }
        public DateTime MWStartDate { get; set; }
        public decimal FinanceAmount { get; set; }

    }

    public class Policy_
    {
        public Guid id { get; set; }
        public Guid tpaBranchId { get; set; }
        public DateTime policySoldDate { get; set; }
        public string hrsUsedAtPolicySale { get; set; }
        public Guid salesPersonId { get; set; }
        public Guid policyBundleId { get; set; }
        public bool dealerPolicy { get; set; }
        public Decimal premium { get; set; }
        public Decimal Emi { get; set; }
        public List<ProductContract_> productContracts { get; set; }

    }

    public class Payment_
    {
        public string refNo { get; set; }
        public bool isSpecialDeal { get; set; }
        public decimal discount { get; set; }
        public decimal dealerPayment { get; set; }
        public bool isPartialPayment { get; set; }
        public Guid paymentModeId { get; set; }
        public Guid paymentTypeId { get; set; }
        public decimal customerPayment { get; set; }
        public string comment { get; set; }
    }

    public class PolicyDetails_
    {
        public Customer_ customer { get; set; }
        public Product_ product { get; set; }
        public Policy_ policy { get; set; }
        public Payment_ payment { get; set; }
        public List<Guid> policyDocIds { get; set; }
        public Guid requestedUser { get; set; }
        public decimal transferFee { get; set; }
        public decimal renewalFee { get; set; }
        public Guid loggedInUserId { get; set; }
        
    }

    public class SavePolicyRequestDto
    {
        public PolicyDetails_ policyDetails { get; set; }
    }

}
