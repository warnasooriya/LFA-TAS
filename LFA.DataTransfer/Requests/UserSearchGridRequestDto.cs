using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class UserSearchGridRequestDto
    {
        public paginationOptionsUserSearchGrid paginationOptionsUserSearchGrid { get; set; }
        public UserSearchGridSearchCriterias UserSearchGridSearchCriterias { get; set; }
        public String type { get; set; }
    }

    public class paginationOptionsUserSearchGrid
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public string sort { get; set; }
    }

    public class UserSearchGridSearchCriterias
    {
        public String firstName { get; set; }
        public String lastName { get; set; }
        public String eMail { get; set; }
        public String mobileNo { get; set; }
    }
}
