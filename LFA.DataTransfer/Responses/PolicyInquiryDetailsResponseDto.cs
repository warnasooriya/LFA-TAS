using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyInquiryDetailsResponseDto
    {
        public policyDetailsToInquiry policyDetails { get; set; }
        public policyEndorsementInquiry policyEnrolmentDetails { get; set; }
        public policyCancelletionInquiry policyCancelationDetails { get; set; }
        public policyRenewalInquiry policyrenewal { get; set; }

    }

    //public class PolicyInquiryDetails_
    //{
    //    public PolicyInquiry_ALL policyDetails { get; set; }
    //}
    //public class PolicyInquiry_ALL
    //{
    //    public policyDetailsToInquiry policyDetail { get; set; }
    //    public policyEndorsementInquiry policyenrolment { get; set; }
    //    public policyCancelletionInquiry policycancelletion { get; set; }
    //    public policyRenewalInquiry policyrenewal { get; set; }
    //}



    public class policyDetailsToInquiry
    {
        public string Customer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerType { get; set; }
        public string UsageTypeId { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public string Email { get; set; }
        public string IDNo { get; set; }
        public string IDType { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string NationalityId { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string AddnSerialNo { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string VINNo { get; set; }
        public string MakeId { get; set; }
        public string TransmissionId { get; set; }
        public string BodyType { get; set; }
        public decimal VehiclePrice { get; set; }
        public decimal ItemPrice { get; set; }//marketprice
        public string PlateNo { get; set; }
        public string ModelId { get; set; }
        public string ModelCode { get; set; }
        public string Variant { get; set; }
        public string EngineCapacity { get; set; }
        public decimal DealerPrice { get; set; }
        public string Category { get; set; }
        public string ModelYear { get; set; }
        public string KmAtPolicySale { get; set; }
        public string RefNo { get; set; }
        public string CylinderCount { get; set; }
        public string FuelType { get; set; }
        public string ItemStatus { get; set; }
        public DateTime ItemPurchasedDate { get; set; }
        public string Aspiration { get; set; }
        public string DriveTypeId { get; set; }
        public string CommodityUsageTypeId { get; set; }
        public string CommodityType { get; set; }
        public string CommodityTypeCode { get; set; }
        public string ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Dealer { get; set; }
        public string DealerLocation { get; set; }
        public DateTime PolicySoldDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public string PaymentModeId { get; set; }
        public string SalesPersonId { get; set; }
        public string PolicyNo { get; set; }
        public string IsSpecialDeal { get; set; }
        public decimal CustomerPayment { get; set; }
        public decimal DealerPayment { get; set; }
        public string DealerPaymentCurrencyTypeId { get; set; }
        public string CustomerPaymentCurrencyTypeId { get; set; }
        public decimal Discount { get; set; }
        public string Comment { get; set; }
        public string SerialNo { get; set; }
        public string InvoiceNo { get; set; }
        public string MWStartDate { get; set; }
        public string MWEndDate { get; set; }
        public bool MWIsAvailable { get; set; }
        public DateTime ExtensionStartDate { get; set; }
        public DateTime ExtensionEndDate { get; set; }
        public decimal GrossWeight { get; set; }
        public string Status { get; set; }


        public  string BusinessName { get; set; }
        public  string BusinessAddress1 { get; set; }
        public  string BusinessAddress2 { get; set; }
        public  string BusinessAddress3 { get; set; }
        public  string BusinessAddress4 { get; set; }
        public  string BusinessTelNo { get; set; }
        public  Nullable<bool> AllTyresAreSame { get; set; }

        //Tyre
        public List<PolicyContractTireProductResponseDto> ContractTireProducts { get; set; }
        public List<InvoiceCodeTireDetailsResponseDto> TireDetails { get; set; }
        public List<PolicyContractTireProductResponseDto> retBundle { get; set; }
        public List<PolicyAdditionalDetailsResponseDto> AdditionalTyreDetails { get; set; }

        public string AditionalModel { get; set; }
        public string AditionalMake { get; set; }
        public string AditionalMilage { get; set; }
        public string AditionalModelYear { get; set; }
        public string AditionalCity { get; set; }
        public string EngineNumber { get; set; }

    }

    public class policyEndorsementInquiry
    {

        public string Category { get; set; }
        public string Customer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerType { get; set; }
        public string UsageType { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public string Email { get; set; }
        public string IDNo { get; set; }
        public string IDTypeId { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string Nationality { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress1 { get; set; }
        public string BusinessAddress2 { get; set; }
        public string BusinessAddress3 { get; set; }
        public string BusinessAddress4 { get; set; }
        public string BusinessTelNo { get; set; }
        public string SerialNo { get; set; }
        public string AddnSerialNo { get; set; }
        public string VINNo { get; set; }
        public string PlateNo { get; set; }
        public string ItemStatusId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public DateTime ItemPurchasedDate { get; set; }
        public string TransmissionId { get; set; }
        public string Variant { get; set; }
        public string CylinderCountId { get; set; }
        public string Aspiration { get; set; }
        public string BodyType { get; set; }
        public string EngineCapacity { get; set; }
        public string FuelType { get; set; }
        public string DriveType { get; set; }
        public string ModelCode { get; set; }
        public string InvoiceNo { get; set; }
        public decimal VehiclePrice { get; set; }//marketprice
        public decimal ItemPrice { get; set; }//marketprice
        public decimal DealerPrice { get; set; }
        public string CommodityType { get; set; }

        public string CommodityUsageType { get; set; }
        public string Product { get; set; }
        public string Dealer { get; set; }
        public string DealerLocation { get; set; }
        public string Contract { get; set; }
        public string ExtensionType { get; set; }
        public string CoverType { get; set; }
        public decimal Premium { get; set; }
        public DateTime PolicySoldDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public string SalesPerson { get; set; }
        public string PolicyNo { get; set; }
        public string HrsUsedAtPolicySale { get; set; }
        public string KmAtPolicySale { get; set; }
        public string PaymentModeId { get; set; }
        public string RefNo { get; set; }
        public string IsSpecialDeal { get; set; }
        public decimal CustomerPayment { get; set; }
        public decimal DealerPayment { get; set; }
        public string DealerPaymentCurrencyTypeId { get; set; }
        public string CustomerPaymentCurrencyTypeId { get; set; }
        public decimal Discount { get; set; }
        public string Comment { get; set; }
        public  DateTime RegistrationDate { get; set; }
        public string IsPolicyEndrosed { get; set; }
        public string IsApproved { get; set; }
        public string MWStartDate { get; set; }
        public string MWEndDate { get; set; }

        public decimal GrossWeight { get; set; }

        public string Status { get; set; }
    }

    public class policyCancelletionInquiry    {

        public string Category { get; set; }
        public string Customer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerType { get; set; }
        public string CylinderCountId { get; set; }
        public string UsageTypeId { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public string Email { get; set; }
        public string IDNo { get; set; }
        public string IDTypeId { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string Nationality { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress1 { get; set; }
        public string BusinessAddress2 { get; set; }
        public string BusinessAddress3 { get; set; }
        public string BusinessAddress4 { get; set; }
        public string BusinessTelNo { get; set; }
        public string SerialNo { get; set; }
        public string AddnSerialNo { get; set; }
        public string VINNo { get; set; }
        public string PlateNo { get; set; }
        public string ItemStatusId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public DateTime ItemPurchasedDate { get; set; }
        public string TransmissionId { get; set; }
        public string Variant { get; set; }
        public string CylinderCount { get; set; }
        public string Aspiration { get; set; }
        public string BodyType { get; set; }
        public string EngineCapacity { get; set; }
        public string FuelType { get; set; }
        public string DriveType { get; set; }
        public string ModelCode { get; set; }
        public string InvoiceNo { get; set; }
        public decimal VehiclePrice { get; set; }
        public decimal ItemPrice { get; set; }//marketprice
        public decimal DealerPrice { get; set; }
        public string CommodityType { get; set; }
        public string ProductId { get; set; }
        public string Dealer { get; set; }
        public string DealerLocation { get; set; }
        public string Contract { get; set; }
        public string ExtensionType { get; set; }
        public string CoverType { get; set; }
        public decimal Premium { get; set; }
        public string Product { get; set; }
        public DateTime PolicySoldDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public string SalesPersonId { get; set; }
        public string PolicyNo { get; set; }
        public string HrsUsedAtPolicySale { get; set; }
        public string KmAtPolicySale { get; set; }
        public string PaymentModeId { get; set; }
        public string RefNo { get; set; }
        public string IsSpecialDeal { get; set; }
        public decimal CustomerPayment { get; set; }
        public decimal DealerPayment { get; set; }
        public string DealerPaymentCurrencyTypeId { get; set; }
        public string CustomerPaymentCurrencyTypeId { get; set; }
        public decimal Discount { get; set; }
        public string Comment { get; set; }
        public string CancelationComment { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedUser { get; set; }
        public string IsPolicyCanceled { get; set; }
        public bool IsPolicyCancele { get; set; }
        public decimal GrossWeight { get; set; }
        public string MWStartDate { get; set; }
        public string MWEndDate { get; set; }

    }

    public class policyRenewalInquiry
    {
        public string Category { get; set; }
        public string Customer { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CustomerType { get; set; }
        public string CylinderCountId { get; set; }
        public string UsageTypeId { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public string Email { get; set; }
        public string IDNo { get; set; }
        public string IDTypeId { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string Nationality { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress1 { get; set; }
        public string BusinessAddress2 { get; set; }
        public string BusinessAddress3 { get; set; }
        public string BusinessAddress4 { get; set; }
        public string BusinessTelNo { get; set; }
        public string SerialNo { get; set; }
        public string AddnSerialNo { get; set; }
        public string VINNo { get; set; }
        public string PlateNo { get; set; }
        public string ItemStatusId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string ModelYear { get; set; }
        public DateTime ItemPurchasedDate { get; set; }
        public string TransmissionId { get; set; }
        public string Variant { get; set; }
        public string CylinderCount { get; set; }
        public string Aspiration { get; set; }
        public string BodyType { get; set; }
        public string EngineCapacity { get; set; }
        public string FuelType { get; set; }
        public string DriveType { get; set; }
        public string ModelCode { get; set; }
        public string InvoiceNo { get; set; }
        public decimal VehiclePrice { get; set; }
        public decimal ItemPrice { get; set; }//marketprice
        public decimal DealerPrice { get; set; }
        public string CommodityType { get; set; }
        public string ProductId { get; set; }
        public string Dealer { get; set; }
        public string DealerLocation { get; set; }
        public string Contract { get; set; }
        public string ExtensionType { get; set; }
        public string CoverType { get; set; }
        public decimal Premium { get; set; }
        public string Product { get; set; }
        public DateTime PolicySoldDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public string SalesPersonId { get; set; }
        public string PolicyNo { get; set; }
        public string HrsUsedAtPolicySale { get; set; }
        public string KmAtPolicySale { get; set; }
        public string PaymentModeId { get; set; }
        public string RefNo { get; set; }
        public string IsSpecialDeal { get; set; }
        public decimal CustomerPayment { get; set; }
        public decimal DealerPayment { get; set; }
        public string DealerPaymentCurrencyTypeId { get; set; }
        public string CustomerPaymentCurrencyTypeId { get; set; }
        public decimal Discount { get; set; }
        public string Comment { get; set; }
        public string IsPolicyRenewed { get; set; }
        public string MWStartDate { get; set; }
        public string MWEndDate { get; set; }
        public decimal GrossWeight { get; set; }
    }
}
