using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    public class ClaimRequestDetailsResponseDto
    {
        public Guid Id { get; set; }
        public List<ClaimItemList> ClaimItemList { get; set; }
        public List<CountryTaxesDe> CountryTaxes { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public decimal AuthorizedClaimAmount { get; set; }

        public string RequestedUser { get; set; }
        public string DealerName { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimNumber { get; set; }
        public string CurrencyCode { get; set; }
        public string Country { get; set; }

        public Complaint Complaint { get; set; }
        public AttachmentsResponseDto Attachments { get; set; }
        public object ServiceHistory { get; set; }

        public Guid CommodityCategoryId { get; set; }
        public Guid MakeId{ get; set; }
        public Guid ModelId { get; set; }

        public Guid DealerId { get; set; }
        public Guid PolicyId { get; set; }
        public Guid PolicyDealerId { get; set; }
        public Guid PolicyCountryId { get; set; }
        public Guid ParentId { get; set; }

        public string ClaimDate { get; set; }
        public string ClaimMileage { get; set; }
        public string ClaimStatus { get; set; }
        public bool IsGoodwillClaim { get; set; }

        public string CustomerName { get; set; }
        public string VINNO { get; set; }
        public string PlateNo { get; set; }
        public string LastServiceMileage { get; set; }
        public  string LastServiceDate { get; set; }
        public  string RepairCenter { get; set; }
        public  string RepairCenterLocation { get; set; }
        public  string Make { get; set; }
        public  string Model { get; set; }
        public string CustomerNameCS { get; set; }
        public string DealerCode { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal totalLiability { get; set; }
        public string Wip { get; set; }
        public decimal FailureMilege { get; set; }
        public string FailureDate { get; set; }
        public string CustomeMobileNo { get; set; }
        public string lastComment { get; set; }
        public string ProductCode { get; set; }
    }
}
