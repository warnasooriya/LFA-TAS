using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    class AddBordxSummeryRequestDto
    {

        public Int64 SNo { get; set; }
        public string UnderWriterYear { get; set; }
        public string ReinsurerName { get; set; }
        public string CedentName { get; set; }
        public string InsuredDetailsCountry { get; set; }
        public string BordxMonth { get; set; }
        public int BordxYear { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public string Status { get; set; }
        public string CoverType { get; set; }
        public string WarrantyType { get; set; }
        public string AmtUsedAtPolicySale { get; set; }
        public string DealerName { get; set; }
        public string Location { get; set; }
        public string Broker { get; set; }

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
        public string Variant { get; set; }

        public string CylinderCount { get; set; }

        public string FourByFour { get; set; }
        public string EngineCapacity { get; set; }
        public string Gvw { get; set; }
        public string GVWCategory { get; set; }
        public string ModelYear { get; set; }

        public string PolicyNo { get; set; }
        public string BookletNo { get; set; }
        public string SystemGeneratedNum { get; set; }

        public DateTime PolicySoldDate { get; set; }
        public DateTime VehiclePurcheseDate { get; set; }
        public DateTime VehicleRegistrationDate { get; set; }

        public DateTime ManfWarrantyStartDate { get; set; }
        public DateTime ManfWarrantyTerminationDate { get; set; }

        public string ManfCoverInMonths { get; set; }
        public string MileageLimitationInKMs { get; set; }
        public DateTime DateOfInsuranceRiskStart { get; set; }
        public DateTime DateOfInsuranceRiskTermination { get; set; }
        public string ExtensionPeriodInMonths { get; set; }
        public string ExtensionPeriodInKms { get; set; }
        public string CutOffKm { get; set; }


        public Decimal GrossPremium { get; set; }
        public Decimal GrossPremiumExTax { get; set; }


        public Decimal SalesCommission { get; set; }
        public Decimal GrossPremiumLessSalesCommission { get; set; }

        public Decimal MarketingFee { get; set; }
        public Decimal InsurerFee { get; set; }
        public Decimal AdminFee { get; set; }
        public Decimal InternalGoodWill { get; set; }
        public Decimal DealerCommission { get; set; }
        public Decimal DocumentFee { get; set; }
        public Decimal ClientBrokerage { get; set; }
        public Decimal InsurerNRPRetention { get; set; }
        public Decimal ManufactureCommission { get; set; }




        public Decimal GrossPremiumLessCommission { get; set; }
        public Decimal NRPIncludingBrokerage { get; set; }
        public Decimal Brokerage { get; set; }
        public Decimal NetAbsoluteRiskPremium { get; set; }


        public Decimal SumInsured { get; set; }

        public Decimal NRPIncludingBrokerageUS { get; set; }
        public Decimal BrokerageUS { get; set; }
        public Decimal NetAbsoluteRiskPremiumUS { get; set; }
        public Decimal ConversionRateUS { get; set; }

        public string StartDateRSA { get; set; }
        public string TerminationDateRSA { get; set; }
        public string PeriodInMonthsRSA { get; set; }
        public string CardNumberRSA { get; set; }
        public string GrossPremiumRSA { get; set; }
        public string NRPRSA { get; set; }
        public string InsurancePolicyNoOther { get; set; }
        public string SalesmanOther { get; set; }
        public string CommentOther { get; set; }
        //references
        public Guid BaseCountryId { get; set; }
        public string BaseCountry { get; set; }
        public string BaseCurrencyName { get; set; }
        public Guid BaseCurrencyId { get; set; }
        public Guid BaseCurrencyPeriodId { get; set; }
        public decimal CurrencyConversionRate { get; set; }
        public string TransactionTypeCode { get; set; }
        public Guid TransactionTypeId { get; set; }
        public Guid ContractId { get; set; }
        public Decimal GrossPremiumBeforeTax { get; set; }
        public Decimal NRP { get; set; }
        public Guid PolicyId { get; set; }
        public int autoId { get; set; }

        public bool BordxInsertion
        {
            get;
            set;
        }
    }
}
