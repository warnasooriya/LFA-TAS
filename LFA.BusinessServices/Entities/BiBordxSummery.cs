using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class BiBordxSummery
    {
        public virtual Guid Id { get; set; }
        public virtual Int64 SNo { get; set; }
        public virtual string UnderWriterYear { get; set; }
        public virtual string ReinsurerName { get; set; }
        public virtual string CedentName { get; set; }
        public virtual string InsuredDetailsCountry { get; set; }
        public virtual string BordxMonth { get; set; }
        public virtual int BordxYear { get; set; }
        public virtual string DealName { get; set; }
        public virtual string DealType { get; set; }
        public virtual string Status { get; set; }
        public virtual string CoverType { get; set; }
        public virtual string WarrantyType { get; set; }
        public virtual string AmtUsedAtPolicySale { get; set; }
        public virtual string DealerName { get; set; }
        public virtual string Location { get; set; }
        public virtual string Broker { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string CoBuyer { get; set; }
        public virtual string Address { get; set; }
        public virtual string POBox { get; set; }
        public virtual string City { get; set; }
        public virtual string Zip { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual string BusinessName { get; set; }
        public virtual string BusinessTel { get; set; }
        public virtual string BusinessAddress { get; set; }
        public virtual string BusinessCountry { get; set; }
        public virtual string BusinessCity { get; set; }
        public virtual string ContactPerson { get; set; }
        public virtual string ContactPersonTel { get; set; }
        public virtual string VINNo { get; set; }
        public virtual string RegNo { get; set; }
        public virtual string Category { get; set; }
        public virtual string Manufacture { get; set; }
        public virtual string Model { get; set; }
        public virtual string Variant { get; set; }
        public virtual string CylinderCount { get; set; }
        public virtual string FourByFour { get; set; }
        public virtual string EngineCapacity { get; set; }
        public virtual string Gvw { get; set; }
        public virtual string GVWCategory { get; set; }
        public virtual string ModelYear { get; set; }
        public virtual string PolicyNo { get; set; }
        public virtual string BookletNo { get; set; }
        public virtual string SystemGeneratedNum { get; set; }
        public virtual DateTime PolicySoldDate { get; set; }
        public virtual DateTime VehiclePurcheseDate { get; set; }
        public virtual DateTime VehicleRegistrationDate { get; set; }
        public virtual DateTime ManfWarrantyStartDate { get; set; }
        public virtual DateTime ManfWarrantyTerminationDate { get; set; }
        public virtual string ManfCoverInMonths { get; set; }
        public virtual string MileageLimitationInKMs { get; set; }
        public virtual DateTime DateOfInsuranceRiskStart { get; set; }
        public virtual DateTime DateOfInsuranceRiskTermination { get; set; }
        public virtual string ExtensionPeriodInMonths { get; set; }
        public virtual string ExtensionPeriodInKms { get; set; }
        public virtual string CutOffKm { get; set; }
        public virtual Decimal GrossPremium { get; set; }
        public virtual Decimal GrossPremiumExTax { get; set; }
        public virtual Decimal SalesCommission { get; set; }
        public virtual Decimal GrossPremiumLessSalesCommission { get; set; }
        public virtual Decimal MarketingFee { get; set; }
        public virtual Decimal InsurerFee { get; set; }
        public virtual Decimal AdminFee { get; set; }
        public virtual Decimal InternalGoodWill { get; set; }
        public virtual Decimal DealerCommission { get; set; }
        public virtual Decimal DocumentFee { get; set; }
        public virtual Decimal ClientBrokerage { get; set; }
        public virtual Decimal InsurerNRPRetention { get; set; }
        public virtual Decimal ManufactureCommission { get; set; }
        public virtual Decimal GrossPremiumLessCommission { get; set; }
        public virtual Decimal NRPIncludingBrokerage { get; set; }
        public virtual Decimal Brokerage { get; set; }
        public virtual Decimal NetAbsoluteRiskPremium { get; set; }
        public virtual Decimal SumInsured { get; set; }
        public virtual Decimal NRPIncludingBrokerageUS { get; set; }
        public virtual Decimal BrokerageUS { get; set; }
        public virtual Decimal NetAbsoluteRiskPremiumUS { get; set; }
        public virtual Decimal ConversionRateUS { get; set; }
        public virtual string StartDateRSA { get; set; }
        public virtual string TerminationDateRSA { get; set; }
        public virtual string PeriodInMonthsRSA { get; set; }
        public virtual string CardNumberRSA { get; set; }
        public virtual string GrossPremiumRSA { get; set; }
        public virtual string NRPRSA { get; set; }
        public virtual string InsurancePolicyNoOther { get; set; }
        public virtual string SalesmanOther { get; set; }
        public virtual string CommentOther { get; set; }
        public virtual Guid BaseCountryId { get; set; }
        public virtual string BaseCountry { get; set; }
        public virtual string BaseCurrencyName { get; set; }
        public virtual Guid BaseCurrencyId { get; set; }
        public virtual Guid BaseCurrencyPeriodId { get; set; }
        public virtual decimal CurrencyConversionRate { get; set; }
        public virtual string TransactionTypeCode { get; set; }
        public virtual Guid TransactionTypeId { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Decimal GrossPremiumBeforeTax { get; set; }
        public virtual Decimal NRP { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual int autoId { get; set; }
        public virtual Guid BordxId { get; set; }
    }
}
