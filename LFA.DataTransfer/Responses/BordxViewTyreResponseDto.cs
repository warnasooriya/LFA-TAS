using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxViewTyreResponseDto
    {
        public Int64 SNo { get; set; }
        public DateTime BDXExtractDate { get; set; }
        public string UnderWriterYear { get; set; }
        public string ReinsurerName { get; set; }
        public string SystemGeneratedNumber { get; set; }
        public string InvoiceCode { get; set; }
        public string InvoiceNumber { get; set; }
        public string CedentName { get; set; }
        public string Bank { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string CoBuyer { get; set; }
        public string Address { get; set; }
        public string POBox { get; set; }
        public string Zip { get; set; }
        public string MobileNumber { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime SystemTransactionDate { get; set; }
        public Int32 SystemPolicyTransactionID { get; set; }
        public string BordxNumber { get; set; }
        public string BordxMonth { get; set; }
        public string BordxYear { get; set; }
        public string Salesman { get; set; }
        public string SalesmanCommision { get; set; }
        public string Commodity { get; set; }
        public string DealType { get; set; }
        public string DealName { get; set; }
        public string NewUsed { get; set; }
        public string DealerName { get; set; }
        public string DealerLocation { get; set; }
        public string Status { get; set; }
        public string CoverType { get; set; }
        public string WarrantyType { get; set; }
        public Decimal KMSAtPolicySale { get; set; }
        public string Insured { get; set; }
        public string VehicleIdentification { get; set; }
        public string EngineNumber { get; set; }
        public string PlateNumber { get; set; }
        public string Category { get; set; }
        public string Manufacture { get; set; }
        public string Model { get; set; }
        public string Variant { get; set; }
        public string CylinderCount { get; set; }
        public string FourByFour { get; set; }
        public string Hybrid { get; set; }
        public string ElectricVehicle { get; set; }
        public string EngineCapacity { get; set; }
        public string Gvw { get; set; }
        public string ModelYear { get; set; }
        public DateTime PolicySoldDate { get; set; }
        public string VehiclePurcheseDate { get; set; }
        public string VehicleRegistrationDate { get; set; }
        public DateTime ManfWarrantyStartDate { get; set; }
        public DateTime ManfWarrantyTerminationDate { get; set; }
        public string CancellationDate { get; set; }
        public string ManufLimitationInHours { get; set; }
        public string MileageLimitationInKMs { get; set; }
        public string ManfCoverHours { get; set; }
        public string FL { get; set; }
        public string FR { get; set; }
        public string RL { get; set; }
        public string RR { get; set; }
        public string SP { get; set; }
        public string TyreBrand { get; set; }
        public Decimal TreadDepth { get; set; }
        public string F_ArticleNumber { get; set; }
        public Int32 NumberofTyresFront { get; set; }
        public string F_Width { get; set; }
        public string F_CrossSection { get; set; }
        public string F_Diameter { get; set; }
        public string F_LoadSpeed { get; set; }
        public string F_DotNumber { get; set; }
        public string R_ArticleNumber { get; set; }
        public Int32 NumberofTyresRear { get; set; }
        public string R_Width { get; set; }
        public string R_CrossSection { get; set; }
        public string R_Diameter { get; set; }
        public string R_LoadSpeed { get; set; }
        public string R_DotNumber { get; set; }
        public DateTime DateOfInsuranceRiskStart { get; set; }
        public DateTime DateOfInsuranceRiskTermination { get; set; }
        public string ExtensionPeriodInMonths { get; set; }
        public Int32 ExtensionDurationInMonths { get; set; }
        public string ExtentionDurationInHours { get; set; }
        public string HrsCutOff { get; set; }
        public string MileageExtensionInKMS { get; set; }
        public string CutOffKm { get; set; }
        public Decimal SumInsured { get; set; }
        public Decimal TotalLiability { get; set; }
        public string MaximumNoofClaims { get; set; }
        public Decimal GrossPremiumExcTax { get; set; }
        public Decimal VAT { get; set; }
        public string SalesTax { get; set; }
        public Decimal GrossPremiumIncTax { get; set; }
        public Decimal MarketingFee { get; set; }
        public Decimal InsurerFee { get; set; }
        public Decimal LicensingFee { get; set; }
        public Decimal InternalGoodWill { get; set; }
        public string ManufactureCommission { get; set; }
        public string ProducerCommision { get; set; }
        public string SalesCommision { get; set; }
        public string DocumentFee { get; set; }
        public Decimal DealerCommission { get; set; }
        public Decimal GrossPremiumLessCommission { get; set; }
        public Decimal NRPRIRetention { get; set; }
        public Decimal NRPInsurerRetention { get; set; }
        public Decimal NetAbsoluteRiskPremium { get; set; }
        public Decimal Brokerage { get; set; }
        public Decimal NRP { get; set; }
        public Decimal ConversionRate { get; set; }
        public Decimal USD_NRP { get; set; }
        

        //--
        public Guid TransactionTypeId { get; set; }
        public string TransactionTypeCode { get; set; }
        public Guid ContractId { get; set; }
        public Decimal GrossPremiumBeforeTax { get; set; }
        public Guid PolicyId { get; set; }
        public DateTime SystemPolicyTransactionDate { get; set; }
        public int autoId { get; set; }
        public Guid BaseCountryId { get; set; }
        public string BaseCountry { get; set; }
        public Decimal CurrencyConversionRate { get; set; }

    }
}
