using System;

namespace TAS.DataTransfer.Requests
{
    public class ChequeSearchGridRequestDto
    {
        public PaginationOptionsSearchGrid paginationOptionsSearchGrid { get; set; }
        public SearchGridSearchCriterias searchGridSearchCriterias { get; set; }
        public String type { get; set; }
    }

    public class SearchGridSearchCriterias
    {
        public String ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public decimal ChequeAmount { get; set; }
    }

}
