using System;

namespace TAS.DataTransfer.Requests
{

    public class CustomerSearchGridRequestDto
    {
        public PaginationOptionsCustomerSearchGrid paginationOptionsCustomerSearchGrid { get; set; }
        public CustomerSearchGridSearchCriterias customerSearchGridSearchCriterias { get; set; }
        public String type { get; set; }

    }

    public class PaginationOptionsCustomerSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class CustomerSearchGridSearchCriterias
    {
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String eMail { get; set; }
        public String mobileNo { get; set; }
        public String businessName { get; set; }
    }
}
