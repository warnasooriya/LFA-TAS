using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class RegionResponseDto
    {
        public Guid Id { get; set; }
        public string RegionName { get; set; }
        public string RegionCode { get; set; }

        public bool IsRegionExists { get; set; }

    }
}
