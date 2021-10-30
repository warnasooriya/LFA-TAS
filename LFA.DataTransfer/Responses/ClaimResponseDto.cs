using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class ClaimResponseDto
    {
        public Guid Id { get; set; }
        public Guid PolicyId { get; set; }
        public Guid PolicyCountryId { get; set; }
        public Guid ClaimCountryId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid CommodityCategoryId { get; set; }
        public Guid MakeId { get; set; }
        public Guid ModelId { get; set; }
        public Guid ClaimCurrencyId { get; set; }
        public Guid CurrencyPeriodId { get; set; }
        public Guid PolicyDealerId { get; set; }
        public Guid ClaimSubmittedDealerId { get; set; }
        public Guid ClaimSubmittedBy { get; set; }
        public Guid LastUpdatedBy { get; set; }
        public Guid StatusId { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimNumber { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public string CustomerComplaint { get; set; }
        public string DealerComment { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public bool IsApproved { get; set; }
        public bool IsInvoiced { get; set; }
        public bool IsBatching { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }

        public bool IsClaimExists { get; set; }
    }
}
