using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class GetAllPoliciesForGridRequestDto
    {
        public int pageNumber;
        public int pageSize;
        public GetAllPoliciesSearchRequest searchQuery;

    }


    public class GetAllPoliciesSearchRequest
    {
        public string CommodityType;
        public string PolicyNo;
        public string SerialNo;
        public string MobileNo;
        public DateTime StartDate;
        public DateTime EndDate;


    }
}
