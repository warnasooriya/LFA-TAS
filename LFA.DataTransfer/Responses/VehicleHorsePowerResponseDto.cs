using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class VehicleHorsePowerResponseDto
    {
        public Guid Id { get; set; }
        public string HorsePower { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool IsVehicleHorsePowerExists { get; set; }

    }
}
