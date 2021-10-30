using System;

namespace TAS.Services.Entities
{
    public  class TempBulkUpload
    {
        public virtual  Guid TempBulkUploadId { get; set; }
        public virtual  Guid TempBulkHeaderId { get; set; }
        public virtual  string FirstName { get; set; }
        public virtual  string LastName { get; set; }
        public virtual  string Title { get; set; }
        public virtual  string Occupation { get; set; }
        public virtual  string MaritalStatus { get; set; }
        public virtual  string Country { get; set; }
        public virtual  string City { get; set; }
        public virtual  string Nationality { get; set; }
        public virtual  string PostalCode { get; set; }
        public virtual  string Email { get; set; }
        public virtual  string MobilePhone { get; set; }
        public virtual  string OtherPhone { get; set; }
        public virtual  DateTime? DateOfBirth { get; set; }
        public virtual  string Gender { get; set; }
        public virtual  string CustomerType { get; set; }
        public virtual  string UsageType { get; set; }
        public virtual  string Address1 { get; set; }
        public virtual  string Address2 { get; set; }
        public virtual  string Address3 { get; set; }
        public virtual  string Address4 { get; set; }
        public virtual  string IDNo { get; set; }
        public virtual  string IDType { get; set; }
        public virtual  DateTime? DrivingIssueDate { get; set; }
        public virtual  string BusinessName { get; set; }
        public virtual  string BusinessAddress1 { get; set; }
        public virtual  string BusinessAddress2 { get; set; }
        public virtual  string BusinessAddress3 { get; set; }
        public virtual  string BusinessAddress4 { get; set; }
        public virtual  string BusinessTelephoneNo { get; set; }
        public virtual  string CommodityTypeCode { get; set; }
        public virtual  string ProductCode { get; set; }
        public virtual  string DealerCode { get; set; }
        public virtual  string DealerLocationCode { get; set; }
        public virtual  string ContractCode { get; set; }
        public virtual  string ExtensionTypeCode { get; set; }
        public virtual  string CoverTypeCode { get; set; }
        public virtual  string SalesPersonCode { get; set; }
        public virtual  string KmAtPolicySale { get; set; }
        public virtual  string HrsAtPolicySale { get; set; }
        public virtual  string Comment { get; set; }
        public virtual  decimal Premium { get; set; }
        public virtual  DateTime? PolicySoldDate { get; set; }
        public virtual  DateTime? PolicyStartDate { get; set; }
        public virtual  DateTime? PolicyEndDate { get; set; }
        public virtual  decimal Discount { get; set; }
        public virtual  string ProviderCode { get; set; }
        public virtual  string SerialNo { get; set; }
        public virtual  string VINNo { get; set; }
        public virtual  string MakeCode { get; set; }
        public virtual  string ModelCode { get; set; }
        public virtual  string CategoryCode { get; set; }
        public virtual  string ItemStatusCode { get; set; }
        public virtual  string CylinderCountCode { get; set; }
        public virtual  string BodyTypeCode { get; set; }
        public virtual  string FuelTypeCode { get; set; }
        public virtual  string AspirationCode { get; set; }
        public virtual  string TransmissionCode { get; set; }
        public virtual  DateTime? ItemPurchasedDate { get; set; }
        public virtual  string EngineCapacityCode { get; set; }
        public virtual  string DriveTypeCode { get; set; }
        public virtual  string CommodityUsageTypeCode { get; set; }
        public virtual  string VariantCode { get; set; }
        public virtual  string PlateNo { get; set; }
        public virtual  string ModelYear { get; set; }
        public virtual  decimal VehiclePrice { get; set; }
        public virtual  DateTime? EntryDateTime { get; set; }
        public virtual  Guid? EntryUserId { get; set; }
        public virtual  bool IsUploaded { get; set; }
        public virtual  int lineId { get; set; }
        //public virtual  int OrderId { get; set; }
        public virtual  string ValidationError { get; set; }
        public virtual  string Colour { get; set; }
        public virtual  bool? IsSpecialDeal { get; set; }
        public virtual  bool? DealerPolicy { get; set; }
        public virtual  DateTime? MWStartDate { get; set; }
        public virtual  string PolicyNo { get; set; }
        public virtual  decimal? GrossWeight { get; set; }
        public virtual  int? ManufacturerWarrantyMonths { get; set; }
        public virtual  string ManufacturerWarrantyKm { get; set; }
        public virtual  DateTime? ManufacturerWarrantyApplicableFrom { get; set; }
        public virtual  int? ExtensionPeriod { get; set; }
        public virtual  string ExtensionMileage { get; set; }
        public virtual  string ContactPersonFirstName { get; set; }
        public virtual  string ContactPersonLastName { get; set; }
        public virtual  string ContactPersonMobileNo { get; set; }
        public virtual  string Dealer { get; set; }
        public virtual  decimal? Deposit { get; set; }
        public virtual  decimal? FinanceAmount { get; set; }
        public virtual  int? LoanPeriod { get; set; }
        public virtual  decimal? PeriodOfCover { get; set; }
        public virtual  decimal? MonthlyEMI { get; set; }
        public virtual  decimal? InterestRate { get; set; }
        public virtual  decimal? GrossPremiumExcludingTAX { get; set; }
        public virtual  string Make { get; set; }
        public virtual  string Model { get; set; }
        public virtual  DateTime? VehicleRegistrationDate { get; set; }

        // additional policy details for tyre

        public virtual string NumberOfTyreCover { get; set; }
        public virtual string TyreBrand { get; set; }
        public virtual string FrontWidth { get; set; }
        public virtual string FrontTyreProfile { get; set; }
        public virtual string FrontRadius { get; set; }
        public virtual string FrontSpeedRating { get; set; }
        public virtual string FrontDot { get; set; }
        public virtual string RearWidth { get; set; }
        public virtual string RearTyreProfile { get; set; }
        public virtual string RearRadius { get; set; }
        public virtual string RearSpeedRating { get; set; }
        public virtual string RearDot { get; set; }


    }
}
