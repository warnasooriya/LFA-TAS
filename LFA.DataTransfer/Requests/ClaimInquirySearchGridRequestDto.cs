using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimInquirySearchGridRequestDto
    {
        public PaginationOptionsClaimInquirySearchGrid paginationOptionsClaimInquirySearchGrid { get; set; }
        public ClaimInquirySearchGridSearchCriterias claimInquirySearchGridSearchCriterias { get; set; }
        public String type { get; set; }
    }

    public class PaginationOptionsClaimInquirySearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class ClaimInquirySearchGridSearchCriterias
    {
        public Guid dealerId { get; set; }
        public String claimNumber { get; set; }
        public string policyId { get; set; }
        public Guid customerId { get; set; }
        public Guid claimId { get; set; }
        public Guid status { get; set; }
        public Guid country { get; set; }

    }
}
