using System;

namespace TAS.DataTransfer.Requests
{
    public class TempBulkUploadRequestDto
    {
        public Guid TempBulkUploadId { get; set; }

        //-------------------------- Customer ------------------------------------------

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Occupation { get; set; }
        public string MaritalStatus { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Nationality { get; set; }
        public string PostalCode { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string OtherPhone { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string CustomerType { get; set; }
        public string UsageType { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string IDType { get; set; }
        public string IDNo { get; set; }
        public string DrivingLicenseIssueDate { get; set; }
        public string BusinessName { get; set; }
        public string BusinessTelephoneNo { get; set; }
        public string BusinessAddress1 { get; set; }
        public string BusinessAddress2 { get; set; }
        public string BusinessAddress3 { get; set; }
        public string BusinessAddress4 { get; set; }
        public string ContactPersonFirstName { get; set; }
        public string ContactPersonLastName { get; set; }
        public string ContactPersonMobileNo { get; set; }


        //public string Password { get; set; }
        //public string ConfirmPassword { get; set; }

        //---------------- Vechicle ---------------------------------------

        public string SerialNo { get; set; }
        public string VINNo { get; set; }
        public string MakeCode { get; set; }
        public string ModelCode { get; set; }
        public string CategoryCode { get; set; }
        public string ItemStatusCode { get; set; }
        public string CylinderCountCode { get; set; }
        public string BodyTypeCode { get; set; }
        public string FuelTypeCode { get; set; }
        public string AspirationCode { get; set; }
        public string TransmissionCode { get; set; }
        public string ItemPurchasedDate { get; set; }
        public string VehicleRegistrationDate { get; set; }

        public string EngineCapacityCode { get; set; }
        public string DriveTypeCode { get; set; }
        public string CommodityUsageTypeCode { get; set; }
        public string VariantCode { get; set; }
        public string PlateNo { get; set; }
        public string ModelYear { get; set; }
        public string VehiclePrice { get; set; }
        public string GrossWeight { get; set; }

        /*
        public string VINNo { get; set; }
        public string MakeCode { get; set; }
        public string ModelCode { get; set; }
        public string CategoryCode { get; set; }
        public string ItemStatusCode { get; set; }
        public string CylinderCountCode { get; set; }
        public string BodyTypeCode { get; set; }
        public string FuelTypeCode { get; set; }
        public string AspirationCode { get; set; }
        public string TransmissionCode { get; set; }
        public string ItemPurchasedDate { get; set; }
        public string EngineCapacityCode { get; set; }
        public string DriveTypeCode { get; set; }
        public string CommodityUsageTypeCode { get; set; }
        public string DealerCurrencyCode { get; set; }
        public string CountryCode { get; set; }
        //public string DealerCode { get; set; }
        public string VariantCode { get; set; }
        public string DealerPrice { get; set; }
        public string PlateNo { get; set; }
        public string ModelYear { get; set; }
        //public string ItemPurchasedDate { get; set; }
        public string VehiclePrice { get; set; }
         */

        //----------------------------------- Policy ------------------------------------

        public string CommodityTypeCode { get; set; }
        public string ProductCode { get; set; }
        public string DealerCode { get; set; }
        public string Dealer { get; set; }
        public string DealerLocation { get; set; }
        public string ContractCode { get; set; }
        public string ExtensionType { get; set; }
        public string CoverTypeCode { get; set; }
        public string SalesPerson { get; set; }
        public string HrsAtPolicySale { get; set; }
        public string KmAtPolicySale { get; set; }
        public string Comment { get; set; }
        public string Premium { get; set; }
        public string PolicySoldDate { get; set; }
        public string PolicyStartDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string Discount { get; set; }
        public string ProviderCode { get; set; }
        public bool IsSpecialDeal { get; set; }
        public bool DealerPolicy { get; set; }
        public string MWStartDate { get; set; }
        public string PolicyNo { get; set; }
        public string ManufacturerWarrantyMonths { get; set; }
        public string ManufacturerWarrantyKm { get; set; }
        public string ManufacturerWarrantyApplicableFrom { get; set; }
        public string ExtensionPeriod { get; set; }
        public string ExtensionMileage  { get; set; }

        public string Deposit { get; set; }
        public string FinanceAmount { get; set; }
        public string LoanPeriod { get; set; }
        public string PeriodOfCover { get; set; }
        public string MonthlyEMI { get; set; }
        public string InterestRate { get; set; }
        public string GrossPremiumExcludingTAX { get; set; }

        public string Make { get; set; }
        public string Model { get; set; }

        /*
        public string CommodityTypeCode { get; set; }
        public string ProductCode { get; set; }
        public string DealerCode { get; set; }
        public string DealerLocationCode { get; set; }
        public string ContractCode { get; set; }
        public string ExtensionTypeCode { get; set; }
        public string PremiumCurrencyTypeCode { get; set; }
        public string CoverTypeCode { get; set; }
        public string SalesPersonCode { get; set; }
        public string DealerPaymentCurrencyTypeCode { get; set; }
        public string CustomerPaymentCurrencyTypeCode { get; set; }
        public string PaymentModeCode { get; set; }
        public string PaymentTypeCode { get; set; }
        public string CustomerCode { get; set; }
        public string TPABranchCode { get; set; }
        public string BordxCode { get; set; }
        public string BordxCountryCode { get; set; }
        public string HrsUsedAtPolicySale { get; set; }
        public string PolicyNo { get; set; }
        public string RefNo { get; set; }
        public string Comment { get; set; }
        public string Premium { get; set; }
        public string DealerPayment { get; set; }
        public string CustomerPayment { get; set; }
        public string IsPreWarrantyCheck { get; set; }
        public string IsSpecialDeal { get; set; }
        public string IsPartialPayment { get; set; }
        //public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public string PolicySoldDate { get; set; }
        public string IsApproved { get; set; }
        public string IsPolicyCanceled { get; set; }
        public string IsPolicyRenewed { get; set; }
        public string PolicyStartDate { get; set; }
        public string PolicyEndDate { get; set; }
        public string Discount { get; set; }
        public string TransferFee { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string BordxNumber { get; set; }
        public string ForwardComment { get; set; }
        public string DealerPolicy { get; set; }
        public string PolicyApprovedBy { get; set; }
        public string PaymentMethodFee { get; set; }
        public string PaymentMethodFeePercentage { get; set; }
        public string GrossPremiumBeforeTax { get; set; }
        public string NRP { get; set; }
        public string TotalTax { get; set; }
        public string EligibilityFee { get; set; }
         */

        //----------------------------------------------------------------

        public DateTime? EntryDateTime { get; set; }
        public Guid? EntryUserId { get; set; }
        public bool IsUploaded { get; set; }
        public int lineId { get; set; }
        public int OrderId { get; set; }


        // Additional tyre details
        public string NumberOfTyreCover { get; set; }
        public string TyreBrand { get; set; }
        public string FrontWidth { get; set; }
        public string FrontTyreProfile { get; set; }
        public string FrontRadius { get; set; }
        public string FrontSpeedRating { get; set; }
        public string FrontDot { get; set; }
        public string RearWidth { get; set; }
        public string RearTyreProfile { get; set; }
        public string RearRadius { get; set; }
        public string RearSpeedRating { get; set; }
        public string RearDot { get; set; }

    }
}
