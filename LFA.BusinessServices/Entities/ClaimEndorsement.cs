using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimEndorsement
    {

        public virtual Guid Id { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual Guid PolicyCountryId { get; set; }
        public virtual Guid ClaimCountryId { get; set; }
        public virtual Guid CustomerId { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid CommodityCategoryId { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual Guid ClaimCurrencyId { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual Guid PolicyDealerId { get; set; }
        public virtual Guid ClaimSubmittedDealerId { get; set; }
        public virtual Guid ClaimSubmittedBy { get; set; }
        public virtual Guid? LastUpdatedBy { get; set; }
        public virtual Guid StatusId { get; set; }
        public virtual Guid ExamineBy { get; set; }
        public virtual Guid ApprovedBy { get; set; }
        public virtual Guid ClaimId { get; set; }


        public virtual string PolicyNumber { get; set; }
        public virtual string ClaimNumber { get; set; }
        public virtual decimal TotalClaimAmount { get; set; }
        public virtual decimal TotalGrossClaimAmount { get; set; }

        public virtual decimal ConversionRate { get; set; }
        public virtual decimal AuthorizedAmount { get; set; }


        public virtual string CustomerComplaint { get; set; }
        public virtual string DealerComment { get; set; }
        public virtual string EngineerComment { get; set; }
        public virtual string Conclution { get; set; }

        public virtual DateTime EntryDate { get; set; }
        public virtual DateTime LastUpdatedDate { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsInvoiced { get; set; }
        public virtual bool IsBatching { get; set; }
        public virtual Guid? GroupId { get; set; }
        public virtual decimal PaidAmount { get; set; }
        public virtual string PaidComment { get; set; }
        public virtual DateTime ApprovedDate { get; set; }
        public virtual bool? IsRejected { get; set; }
        public virtual bool? EndorsementIsApproved { get; set; }
        public virtual Guid? EndorsementApprovedOrRejectedBy { get; set; }
        public virtual DateTime EndorsementApprovedOrRejectedDate { get; set; }

    }
}
