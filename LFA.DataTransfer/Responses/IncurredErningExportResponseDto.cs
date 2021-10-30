using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace TAS.DataTransfer.Responses
{
    public class IncurredErningExportResponseDto
    {
        public List<LostRatioSummary> LostRatioSummary { get; set; }

        public List<LostRatioSummaryOther> LostRatioSummaryOther { get; set; }
        public string TpaName { get; set; }
        public string tpaLogo { get; set; }
    }

    public class LostRatioSummary
    {
        public string Country { get; set; }
        public string DealerContract { get; set; }
        public string ContractYear { get; set; }
        public string WarrantyType { get; set; }
        public string CoverType { get; set; }
        public int PolicyCount { get; set; }
        public Decimal GrossPremium { get; set; }
        public Decimal NetPremium { get; set; }
        public Decimal ErnedGrossPremium { get; set; }
        public Decimal ErnedNetPremium { get; set; }
        public Decimal RiskCompleted { get; set; }
        public int ReInPaidClaimsCount { get; set; }
        public Decimal ReInPaidClaimsValue { get; set; }
        public int ReInPaidReservedClaimsCount { get; set; }
        public Decimal ReInPaidReservedClaimsValue { get; set; }
        public int SAPaidClaimsCount { get; set; }
        public int SAPaidClaimsValue { get; set; }
        public int SAPaidReservedClaimsCount { get; set; }
        public int SAPaidReservedClaimsValue { get; set; }

    }
    public class LostRatioSummaryOther
    {
        public DateTime EarnedDate { get; set; }
        public DateTime ClaimDate { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string CylinderCount { get; set; }
        public Decimal EngineCapacityNumber { get; set; }
        public DateTime BordxStartDate { get; set; }
        public DateTime BordxEndDate { get; set; }
        public string EXTmonth { get; set; }
        public string WarrantyType { get; set; }
        public string CoverType { get; set; }
        public int PolicyCount { get; set; }
        public Decimal GrossPremium { get; set; }
        public Decimal NetPremium { get; set; }
        public Decimal ErnedGrossPremium { get; set; }
        public Decimal ErnedNetPremium { get; set; }
        public Decimal RiskCompleted { get; set; }
        public int ReInPaidClaimsCount { get; set; }
        public Decimal ReInPaidClaimsValue { get; set; }
        public int ReInPaidReservedClaimsCount { get; set; }
        public Decimal ReInPaidReservedClaimsValue { get; set; }
        public int SAPaidClaimsCount { get; set; }
        public int SAPaidClaimsValue { get; set; }
        public int SAPaidReservedClaimsCount { get; set; }
        public int SAPaidReservedClaimsValue { get; set; }

    }

}
