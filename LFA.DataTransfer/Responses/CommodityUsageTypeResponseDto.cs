using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
 [Serializable]
    public class CommodityUsageTypeResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool IsCommodityUsageTypeExists { get; set; }

    }
}
