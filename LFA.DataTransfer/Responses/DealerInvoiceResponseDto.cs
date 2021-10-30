using System;

namespace TAS.DataTransfer.Responses
{

    public class DealerInvoiceResponseDto
    {

        public Int64 SNo { get; set; }
        public string UnderWriterYear { get; set; }
        public string ReinsurerName { get; set; }
        public string CedentName { get; set; }
        public string InsuredDetailsCountry { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string Status { get; set; }
        public string CoverType { get; set; }
        public string WarrantyType { get; set; }
        public string AmtUsedAtPolicySale { get; set; }
        public string DealerName { get; set; }
        public string Location { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CoBuyer { get; set; }
        public string Address { get; set; }
        public string POBox { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string MobileNumber { get; set; }

        public string BusinessName { get; set; }
        public string BusinessTel { get; set; }
        public string BusinessAddress { get; set; }
        public string BusinessCountry { get; set; }
        public string BusinessCity { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonTel { get; set; }

        public string VINNo { get; set; }
        public string RegNo { get; set; }
        public string Category { get; set; }
        public string Manufacture { get; set; }
        public string Model { get; set; }
        public string CylinderCount { get; set; }
        public string FourByFour { get; set; }
        public string EngineCapacity { get; set; }
        public string ModelYear { get; set; }
        public string PolicyNo { get; set; }
        public string BookletNo { get; set; }
        public string SystemGeneratedNum { get; set; }
        public DateTime VehicleRegistrationDate { get; set; }
        public DateTime ManfWarrantyTerminationDate { get; set; }
        public string ManfCoverInMonths { get; set; }
        public string MileageLimitationInKMs { get; set; }
        public DateTime DateOfInsuranceRiskStart { get; set; }
        public DateTime DateOfInsuranceRiskTermination { get; set; }
        public string ExtensionPeriodInMonths { get; set; }
        public string ExtensionPeriodInKms { get; set; }
        public Decimal GrossPremium { get; set; }
        public decimal CurrencyConversionRate { get; set; }

    }
}
