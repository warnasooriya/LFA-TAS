using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ContractRequestDto
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid DealerId { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid ProductId { get; set; }
        public Guid LinkDealId { get; set; }
        public string DealName { get; set; }
        public string DealType { get; set; }
        public Guid ItemStatusId { get; set; }
        public Guid InsurerId { get; set; }
        public Guid ReinsurerId { get; set; }
        public Guid WarrantyTypeId { get; set; }
        public bool IsPromotional { get; set; }
        public bool IsAutoRenewal { get; set; }
        public bool DiscountAvailable { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Remark { get; set; }
        public decimal PremiumTotal { get; set; }
        public decimal GrossPremium { get; set; }
        public decimal ClaimLimitation { get; set; }
        public decimal LiabilityLimitation { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public Guid CommodityUsageTypeId { get; set; }

        public bool ContractInsertion
        {
            get;
            set;
        }

    }
}
