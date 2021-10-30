using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CommodityRespondDto
    {
        public Guid CommodityTypeId { get; set; }
        public string CommodityTypeDescription { get; set; }
        public string DisplayDescription { get; set; }
        public string CommonCode { get; set; }
        public string CommodityCode { get; set; }


    }
}
