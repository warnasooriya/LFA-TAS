using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PolicySearchInquiryGridRequestDto
    {
        public PaginationOptionsPolicySearchInquiryGrid paginationOptionsPolicySearchInquiryGrid { get; set; }
        public PolicySearchInquiryGridSearchCriterias policySearchInquiryGridSearchCriterias { get; set; }
        public String type { get; set; }
        public Guid userId { get; set; }
    }

    public class PaginationOptionsPolicySearchInquiryGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class PolicySearchInquiryGridSearchCriterias
    {
        public Guid commodityTypeId { get; set; }
        public Guid CategoryTypeId { get; set; }
        public Guid Country { get; set; }
        public Guid DealerId { get; set; }
        public String policyNo { get; set; }
        public String serialNo { get; set; }
        public String CustomerName { get; set; }
        public String mobileNo { get; set; }
        public DateTime policyStartDate { get; set; }
        public DateTime policyEndDate { get; set; }
        public DateTime policySoldDateFrom { get; set; }
        public DateTime policySoldDateTo { get; set; }

       

    }
}
