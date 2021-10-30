using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class VehicleWeightResponseDto
    {
        public  Guid Id { get; set; }
        public  string VehicleWeightDescription { get; set; }
        public string VehicleWeightCode { get; set; }
        public  decimal WeightFrom { get; set; }
        public  decimal WeightTo { get; set; }
        public  DateTime EntryDateTime { get; set; }
        public  Guid EntryUser { get; set; }

        public bool IsVehicleWeightExists { get; set; }
    }
}
