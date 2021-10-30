using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CommodityCategoryResponseDto
    {
        public Guid CommodityCategoryId { get; set; }
        public string CommodityCategoryCode { get; set; }
        public string CommodityCategoryDescription { get; set; }
        public int Length { get; set; }

        public bool IsCommodityCategoryExists { get; set; }

    }
}
