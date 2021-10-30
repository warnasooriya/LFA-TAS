using System;

namespace TAS.DataTransfer.Responses
{
    public class ClaimAuthorizationReportData
    {
        public string DealerName { get; set; }
        public string ClaimNumber { get; set; }
        public string CountryCity { get; set; }
        public string CustomerName { get; set; }
        public string BookletNumber { get; set; }
        public string PolicyNo { get; set; }
        public string Status { get; set; }
        public string PlateNo { get; set; }
        public string VINNo { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public DateTime FailureDate { get; set; }
        public decimal ClaimMileageKm { get; set; }
        public string CustomerComplaint { get; set; }
        public string DealerComment { get; set; }
        public string EngineerComment { get; set; }
        public string ClaimSubmittedUser { get; set; }
        public string ClaimApprovedUser { get; set; }
        public DateTime ApprovedDate { get; set; }
        public DateTime ClaimDate { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public decimal TotalPayable { get; set; }

    }

    public class ClaimAuthorizationReportDetailData
    {
        public string PartName { get; set; }
        public string PartNumber { get; set; }
        public string ItemCode { get; set; }
        public Guid ParentId { get; set; }
        public decimal AuthorizedAmount { get; set; }
        public decimal ConversionRate { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GoodWillAmount { get; set; }
        public bool IsApproved { get; set; }
        public string Remark { get; set; }
        public decimal TotalGrossPrice { get; set; }
        public decimal TotalPrice { get; set; }

    }
    
}
