using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimInvoiceEntryClaimSearchGridRequestDto
    {
        public paginationOptionsClaimInvoiceEntryClaimSearchGrid paginationOptionsClaimInvoiceEntryClaimSearchGrid { get; set; }
        public claimInvoiceEntryClaimSearchGridSearchCriterias claimInvoiceEntryClaimSearchGridSearchCriterias { get; set; }
        public String type { get; set; }
    }

    public class paginationOptionsClaimInvoiceEntryClaimSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class claimInvoiceEntryClaimSearchGridSearchCriterias
    {
        public String ClaimNumber { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid DealerId { get; set; }

    }
}
