using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBatchingSearchGridRequestDto
    {
        public paginationOptionsClaimBatchingSearchGrid paginationOptionsClaimBatchingSearchGrid { get; set; }
        public claimBatchingSearchGridSearchCriterias claimBatchingSearchGridSearchCriterias { get; set; }
        public String type { get; set; }
    }

    public class paginationOptionsClaimBatchingSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class claimBatchingSearchGridSearchCriterias
    {
        public String BatchNumber { get; set; }
        //public decimal TotalClaimAmount { get; set; }
        //public DateTime LastUpdatedDate { get; set; }

    }

}
 