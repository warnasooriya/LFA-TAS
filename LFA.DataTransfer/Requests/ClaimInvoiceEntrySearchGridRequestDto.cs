using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimInvoiceEntrySearchGridRequestDto
    {
        public paginationOptionsClaimInvoiceEntrySearchGrid paginationOptionsClaimInvoiceEntrySearchGrid { get; set; }
        public ClaimInvoiceEntrySearchGridSearchCriterias ClaimInvoiceEntrySearchGridSearchCriterias { get; set; }
        public String type { get; set; }
    }

    public class paginationOptionsClaimInvoiceEntrySearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class ClaimInvoiceEntrySearchGridSearchCriterias
    {
        public String dealer { get; set; }
        public String invoiceNo { get; set; }
        public DateTime invoiceDate { get; set; }
       
    }
}
