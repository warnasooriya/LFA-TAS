using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ProductSearchInClaimSubmissionIloeRequestDto
    {
        public PaginationOptionsProductSearchGrid paginationOptionsProductSearchGrid { get; set; }
        public ProductSearchGridSearchCriterias productSearchGridSearchCriterias { get; set; }
        public Guid userId { get; set; }
        public string userType { get; set; }
        public Guid productId { get; set; }
    }

    public class PaginationOptionsProductSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public object sort { get; set; }
    }

    public class ProductSearchGridSearchCriterias
    {
        public string policyNo { get; set; }
        public string serialNo { get; set; }
        public string mobileNumber { get; set; }
        public string customerName { get; set; }
    }

}
