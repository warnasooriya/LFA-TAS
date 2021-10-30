using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimSearchGridRequestDto
    {
        public PaginationOptionsClaimSearchGrid paginationOptionsClaimSearchGrid { get; set; }
        public ClaimSearchGridSearchCriterias claimSearchGridSearchCriterias { get; set; }
        public String type { get; set; }
    }

    public class PaginationOptionsClaimSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class ClaimSearchGridSearchCriterias
    {
        public Guid commodityTypeId { get; set; }
        public String claimNumber { get; set; }
        public Guid policyId { get; set; }
        public Guid customerId { get; set; }
        public Guid claimId { get; set; }
    }
}
