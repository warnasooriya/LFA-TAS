using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBatchGroupSearchGridRequestDto
    {
        public paginationOptionsClaimBatchGroupSearchGrid paginationOptionsClaimBatchGroupSearchGrid { get; set; }
        public claimBatchGroupSearchGridSearchCriterias claimBatchGroupSearchGridSearchCriterias { get; set; }
        public String type { get; set; }
    }

    public class paginationOptionsClaimBatchGroupSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class claimBatchGroupSearchGridSearchCriterias
    {
        public String GroupName { get; set; }
        //public decimal TotalClaimAmount { get; set; }
        //public DateTime LastUpdatedDate { get; set; }

    }
}
