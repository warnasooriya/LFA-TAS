using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
   public class PolicySearchResponseDto
    {
        public PolicySearchResponseDto() { }
        public Guid Id { get; set; }
        public string CommodityType { get; set; }
        public string PolicyNo { get; set; }
        public string SerialNo { get; set; }
        public string MobileNo { get; set; }
        public string PolicySoldDate { get; set; }
        public DateTime EntryDateTime { get; set; }
    }

}
