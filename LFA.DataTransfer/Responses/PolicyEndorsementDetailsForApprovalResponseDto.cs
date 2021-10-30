using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    public class PolicyEndorsementDetailsForApprovalResponseDto
    {
        public PolicyEndorsementDetails policyDetails { get; set; }
        public PolicyEndorsementDetails endorsementDetails { get; set; }

    }
    
    
    public class Customer_E
    {
        public Guid customerId { get; set; }
        public string customerType { get; set; }
        public string usageType { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime dateOfBirth { get; set; }
        public string gender { get; set; }
        public string idType { get; set; }
        public string idNo { get; set; }
        public DateTime idIssueDate { get; set; }
        public string nationality { get; set; }
        public string country { get; set; }
        public string city { get; set; }
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

    public class Product_E
    {
        public Guid id { get; set; }
        public string commodityType { get; set; }
        public string product { get; set; }
        public string productCode { get; set; }
        public string productTypeCode { get; set; }
        public string dealer { get; set; }
        public string dealerLocation { get; set; }
        public string category { get; set; }
        public string serialNumber { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string modelYear { get; set; }
        public string invoiceNo { get; set; }
        public string additionalSerial { get; set; }
        public string itemStatus { get; set; }
        public string commodityUsageType { get; set; }
        public DateTime itemPurchasedDate { get; set; }
        public decimal dealerPrice { get; set; }
        public decimal itemPrice { get; set; }
        public string variant { get; set; }
        public string engineCapacity { get; set; }
        public string cylinderCount { get; set; }
        public string fuelType { get; set; }
        public string transmissionType { get; set; }
        public string bodyType { get; set; }
        public string driveType { get; set; }
        public string aspirationType { get; set; }
        public string dealerPaymentCurrencyType { get; set; }
        public string customerPaymentCurrencyType { get; set; }
        public DateTime registrationDate { get; set; }
        public string engineNumber { get; set; }
    }

    public class Contract_E
    {
        public string Country { get; set; }
        public string Dealer { get; set; }
        public string CommodityType { get; set; }
        public string CommodityCategory { get; set; }
        public string Product { get; set; }
        public Guid LinkDealId { get; set; }
        public string DealName { get; set; }
        public Guid DealType { get; set; }
        public string ItemStatus { get; set; }
        public string Insurer { get; set; }
        public string Reinsurer { get; set; }
        public bool IsAutoRenewal { get; set; }
        public bool IsPromotional { get; set; }
        public bool DiscountAvailable { get; set; }
        public bool IsActive { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Remark { get; set; }
        public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public int GrossPremium { get; set; }
        public int ClaimLimitation { get; set; }
        public int LiabilityLimitation { get; set; }
        public int PremiumTotal { get; set; }
        public string CommodityUsageType { get; set; }
        public Guid Id { get; set; }
     
    }

    public class ExtensionType_E
    {
        public Guid Id { get; set; }
        public string ExtensionName { get; set; }
        public int Km { get; set; }
        public int Month { get; set; }
        public string CommodityType { get; set; }
        public string Product { get; set; }
        public int Hours { get; set; }
        public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public bool IsExtensionTypeExists { get; set; }
    }

    public class CoverType_E
    {
        public Guid Id { get; set; }
        public string WarrantyTypeDescription { get; set; }
        public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public bool IsWarrantyTypeExists { get; set; }
    }

    public class ProductContract_E
    {
        public string Product { get; set; }
        public string ParentProduct { get; set; }
        public string Contract { get; set; }
        public object ExtensionType { get; set; }
        public object CoverType { get; set; }
        public Guid Id { get; set; }

        public string PolicyNo { get; set; }
        public List<Contract_E> Contracts { get; set; }
        public List<ExtensionType_E> ExtensionTypes { get; set; }
        public List<CoverType_E> CoverTypes { get; set; }
        public decimal Premium { get; set; }
        public string PremiumCurrencyName { get; set; }
        public Guid PremiumCurrencyTypeId { get; set; }
        public string Name { get; set; }
        public bool RSA { get; set; }

        public string PolicyStartDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string RefNo { get; set; }
        public string HrsUsedAtPolicySale { get; set; }

    }

    public class Policy_E
    {
        public Guid id { get; set; }
        public string tpaBranch { get; set; }
        public DateTime policySoldDate { get; set; }
        public string hrsUsedAtPolicySale { get; set; }
        public string salesPerson { get; set; }
        public bool dealerPolicy { get; set; }
        public Decimal premium { get; set; }
        public List<ProductContract_E> productContracts { get; set; }

    }

    public class Payment_E
    {
        public string refNo { get; set; }
        public bool isSpecialDeal { get; set; }
        public decimal discount { get; set; }
        public decimal dealerPayment { get; set; }
        public bool isPartialPayment { get; set; }
        public string paymentMode { get; set; }
        public decimal customerPayment { get; set; }
        public string comment { get; set; }
    }

    public class PolicyDetails_E
    {
        public Customer_E customer { get; set; }
        public Product_E product { get; set; }
        public Policy_E policy { get; set; }
        public Payment_E payment { get; set; }
        public List<Guid> policyDocIds { get; set; }
        public Guid requestedUser { get; set; }
    }

    public class PolicyEndorsementDetails
    {
        public PolicyDetails_E policyDetails { get; set; }
    }
}
