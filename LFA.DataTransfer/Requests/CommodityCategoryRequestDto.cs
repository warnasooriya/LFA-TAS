using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class CommodityCategoryRequestDto
    {
        public Guid CommodityCategoryId { get; set; }
        public string CommodityCategoryCode { get; set; }
        public string CommodityCategoryDescription { get; set; }
        public Guid CommodityTypeId { get; set; }
        public int Length { get; set; }

        public bool CommodityCategoryInsertion
        {
            get;
            set;
        }

    }
}
