using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class VehicleColorResponseDto
    {
        public Guid Id { get; set; }
        public string VehicleColorCode { get; set; }
        public string Color { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsVehicleColorExists { get; set; }

    }
}
