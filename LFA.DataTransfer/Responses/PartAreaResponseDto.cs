using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class PartAreaResponseDto
    {
        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid CommodityCategoryId { get; set; }
        public string PartAreaCode { get; set; }
        public string PartAreaName { get; set; }

        public bool IsPartAreaExists { get; set; }
    }
}
