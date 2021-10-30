using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PriceSearchGridRequestDto
    {
        public PaginationOptionsPriceSearchGrid paginationOptionsPriceSearchGrid { get; set; }
        public PriceSearchGridSearchCriterias priceSearchGridSearchCriterias { get; set; }
        public Guid dealerId { get; set; }
    }

    public class PaginationOptionsPriceSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class PriceSearchGridSearchCriterias
    {        
        public Guid dealerid { get; set; }
        public Guid countryid { get; set; }
    }
}
