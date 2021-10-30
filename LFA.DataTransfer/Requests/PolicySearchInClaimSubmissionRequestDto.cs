
using System;
namespace TAS.DataTransfer.Requests
{
    public class PolicySearchInClaimSubmissionRequestDto
    {
        public PaginationOptionsPolicySearchGrid paginationOptionsPolicySearchGrid { get; set; }
        public PolicySearchGridSearchCriterias policySearchGridSearchCriterias { get; set; }
        public Guid userId { get; set; }
        public string userType { get; set; }
    }

    public class PaginationOptionsPolicySearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public object sort { get; set; }
    }

    public class PolicySearchGridSearchCriterias
    {
        public string policyNo { get; set; }
        public string serialNo { get; set; }

        public string mobileNumber { get; set; }

        public string customerName { get; set; }
        public string plateNo { get; set; }
    }


}
