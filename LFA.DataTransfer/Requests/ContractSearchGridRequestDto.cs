using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ContractSearchGridRequestDto
    {
        public GridPaginationOptions paginationOptionsContractSearchGrid { get; set; }
        public ContractSearchGridSearchCriterias contractSearchGridSearchCriterias { get; set; }
    }

    public class GridPaginationOptions
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }

    public class ContractSearchGridSearchCriterias
    {
        public Guid countryId { get; set; }
        public Guid dealerId { get; set; }
        public Guid productId { get; set; }
        public String dealName { get; set; }

    }
}
