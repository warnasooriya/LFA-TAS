using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealerDiscountSchemesSearchRequestDto
    {
        public PaginationOptionsDealerDiscountSearchGrid paginationOptionsDealerDiscountSearchGrid { get; set; }
        public DealerDiscountSearchGridSearchCriterias dealerDiscountSearchGridSearchCriterias { get; set; }
    }

    public class PaginationOptionsDealerDiscountSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public object sort { get; set; }
    }

    public class DealerDiscountSearchGridSearchCriterias
    {
        public Guid id { get; set; }
        public string itemType { get; set; }
        public Guid countryId { get; set; }
        public Guid dealerId { get; set; }
        public Guid makeId { get; set; }
        public Guid discounSchemeId { get; set; }

        public bool isActive { get; set; }
    }

}
