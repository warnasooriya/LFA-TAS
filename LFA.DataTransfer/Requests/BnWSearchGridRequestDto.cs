using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BnWSearchGridRequestDto
    {
        public PaginationOptionsBnWSearchGrid paginationOptionsBnWSearchGrid { get; set; }
        public BnWSearchGridSearchCriterias bnWSearchGridSearchCriterias { get; set; }

    }

    public class PaginationOptionsBnWSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class BnWSearchGridSearchCriterias
    {
        public String serialNo { get; set; }
        public Guid make { get; set; }
        public Guid model { get; set; }

        public String invoiceno { get; set; }

    }
}

