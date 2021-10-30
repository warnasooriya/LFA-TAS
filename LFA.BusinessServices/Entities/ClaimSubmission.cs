using System;

namespace TAS.Services.Entities
{
    public class ClaimSubmission
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
        public virtual string PolicyNumber { get; set; }
        public virtual string ClaimNumber { get; set; }
        public virtual decimal TotalClaimAmount { get; set; }
        public virtual decimal ConversionRate { get; set; }
        public virtual DateTime ClaimDate { get; set; }
        public virtual decimal ClaimMileage { get; set; }

        public virtual string VINNo { get; set; }
        public virtual string CustomerName { get; set; }
        public virtual string PlateNo { get; set; }
        public virtual decimal LastServiceMileage { get; set; }
        public virtual DateTime LastServiceDate { get; set; }
        public virtual string RepairCenter { get; set; }
        public virtual string RepairCenterLocation { get; set; }
        public virtual decimal FailureMileage { get; set; }

        public virtual string CustomerComplaint { get; set; }
        public virtual string DealerComment { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual DateTime LastUpdatedDate { get; set; }

        public virtual DateTime FailureDate { get; set; }
        public virtual Guid RejectionTypeId { get; set; }
        public virtual string Wip { get; set; }
        public virtual string MobileNo { get; set; }



    }
}
