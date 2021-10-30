using System;

namespace TAS.DataTransfer.Requests
{
    public class DealerInvoiceCodeSearchRequestDto
    {
        public PaginationOptionsDealerDealerInvoiceSearchGrid paginationOptionsDealerDealerInvoiceSearchGrid { get; set; }
        public DealerInvoiceSearchSearchGridSearchCriterias dealerInvoiceSearchGridSearchCriterias { get; set; }
        public DealerInvoiceFilterInformation dealerInvoiceFilterInformation { get; set; }
    }

    public class PaginationOptionsDealerDealerInvoiceSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class DealerInvoiceSearchSearchGridSearchCriterias
    {
        public DateTime? date { get; set; }
        public string plateNo { get; set; }
        public string code { get; set; }
    }

    public class DealerInvoiceFilterInformation
    {
        public Guid dealerId { get; set; }
        public Guid dealerBranchId { get; set; }
        public Guid countryId { get; set; }
        public Guid cityId { get; set; }

    }

}
