using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimBordxExportResponseDto
    {

        public ClaimBordxHeader ClaimBordxHeader { get; set; }

        public List<ClaimSummary> ClaimSummaries { get; set; }

        public List<ClaimMonthAndYear> ClaimMonthAndYears { get; set; }

        public List<PaymentSchedule> PaymentSchedule { get; set; }

        public List<CountryWise> CountryWise { get; set; }
    }

    public class ClaimBordxHeader
    {
        public string Reinsurer { get; set; }
        public string Country { get; set; }
        public string CedentName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string FileName { get; set; }
    }

    public class ClaimSummary
    {
        public string TransactionNumber { get; set; }
        public int ClaimBordxYear { get; set; }
        public string ClaimBordxMonth { get; set; }
        public string ClaimBordxReferneceNumber { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal Outstanding { get; set; }
        public string Remarks { get; set; }

    }

    public class ClaimMonthAndYear
    {
        public string UWYear { get; set; }
        public int PaidClaimCount { get; set; }
        public decimal PaiClaimAmountInLocal { get; set; }
        public decimal PaiClaimAmount { get; set; }

    }

    public class PaymentSchedule
    {
        public string CoverHolder { get; set; }
        public string MonthOfAccount { get; set; }
        public string YearOfAccount { get; set; }
        public int ContractYear { get; set; }
        public string Payee { get; set; }
        public string DateOfClaim { get; set; }
        public string DateOfLoss { get; set; }
        public DateTime ManfWarrantyExpiryDate { get; set; }
        public decimal KmatDateofLoss { get; set; }
        public int ManufacturerWarrantyCutoffKms { get; set; }
        public int ExtWarrantyExpiryKm { get; set; }
        public string ClaimRef { get; set; }
        public string Certchas { get; set; }
        public string Insured { get; set; }
        public decimal PaiClaimAmountInLocal { get; set; }
        public decimal PaiClaimAmount { get; set; }
        public string DatePremiumWasPaid { get; set; }
        public string BordxNumber { get; set; }
        public string BordxMonth { get; set; }
    }

    public class CountryWise
    {
        public string SerialNo { get; set; }
        public string Dealer { get; set; }
        public string BordxMonthYear { get; set; }
        public string UWYear { get; set; }
        public string TypePolicy { get; set; }
        public string PolicyNo { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal? EngineSize { get; set; }
        public string ChassisNo { get; set; }
        public string ClaimNo { get; set; }
        public string ServiceCenter { get; set; }
        public string Location { get; set; }
        public decimal? Mileage { get; set; }
        public string Letter { get; set; }
        public string Number { get; set; }
        public string FaultCode { get; set; }
        public string FailedComponent { get; set; }
        public string CauseOfFailure { get; set; }
        public string ClaimStatus { get; set; }
        public string Sorting { get; set; }
        public decimal? Labour { get; set; }
        public decimal? LocalLabour { get; set; }
        public decimal? Part { get; set; }
        public decimal? LocalPart { get; set; }
        public decimal? AuthAmt { get; set; }
        public decimal? LocalAuthAmt { get; set; }
        public decimal? Variance { get; set; }
        public decimal? LocalVariance { get; set; }
        public decimal? GapGoodWill { get; set; }
        public decimal? LocalGapGoodWill { get; set; }
        public decimal? Authorized { get; set; }
        public decimal? LocalAuthorized { get; set; }
        public decimal? Inprogress { get; set; }
        public decimal? LocalInprogress { get; set; }
        public decimal? Paid { get; set; }
        public decimal? LocalPaid { get; set; }
        public decimal? Over180 { get; set; }
        public decimal? LocalOver180 { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public string DateClaimPaid { get; set; }
        public decimal? TotalClaim { get; set; }
        public decimal? LocalTotalClaim { get; set; }
        public decimal? TotalClaimToReinsurance { get; set; }
        public decimal? LocalTotalClaimToReinsurance { get; set; }
        public string UnderWriter { get; set; }
        public DateTime? FailureDate { get; set; }
        public DateTime? ClaimDate { get; set; }
        public int? ManufacturerWarrantyCutoffKms { get; set; }
        public int? ExtWarrantyExpiryKm { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyExpiryDate { get; set; }

        public string CountryName { get; set; }
        public Guid CountrId { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal ItemConversionRate { get; set; }
        public int endosed { get; set; }
    }
}
