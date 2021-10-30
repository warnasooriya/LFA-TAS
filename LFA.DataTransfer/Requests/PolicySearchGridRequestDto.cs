using System;

namespace TAS.DataTransfer.Requests
{
    public class PolicySearchGridRequestDto
    {
        public PaginationOptionsPolicySearchGridClaimSubmission paginationOptionsPolicySearchGrid { get; set; }
        public PolicySearchGridSearchCriteriasClaimSubmission policySearchGridSearchCriterias { get; set; }
        public String type { get; set; }
        public Guid userId { get; set; }
    }

    public class PaginationOptionsPolicySearchGridClaimSubmission
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class PolicySearchGridSearchCriteriasClaimSubmission
    {
        public Guid commodityTypeId { get; set; }
        public String policyNo { get; set; }
        public String serialNo { get; set; }
        public String mobileNo { get; set; }
        public DateTime policyStartDate { get; set; }
        public DateTime policyEndDate { get; set; }

    }
}
