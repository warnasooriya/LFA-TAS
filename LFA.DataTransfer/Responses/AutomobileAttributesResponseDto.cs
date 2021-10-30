using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class AutomobileAttributesResponseDto
    {
        public object DriveTypes { get; set; }
        public object commodityUsageTypes { get; set; }
        public object engineCapacities { get; set; }
        public object cylinderCounts { get; set; }
        public object fuelTypes { get; set; }
        public object transmissionTypes { get; set; }
        public object bodyTypes { get; set; }
        public object aspirationTypes { get; set; }

    }
}
