using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealerLabourChargeSearchRequestDto
    {
        public PaginationOptionsDealerLabourChargeSearchGrid paginationOptionsDealerLabourChargeSearchGrid { get; set; }
        public DealerLabourChargeSearchGridSearchCriterias dealerLabourChargeSearchGridSearchCriterias { get; set; }
    }

    public class PaginationOptionsDealerLabourChargeSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public object sort { get; set; }
    }

    public class DealerLabourChargeSearchGridSearchCriterias
    {       
        public Guid dealerId { get; set; }
        
    }
}
