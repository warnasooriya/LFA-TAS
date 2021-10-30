using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class VehicleWeightRequestDto
    {
        public Guid Id { get; set; }
        public string VehicleWeightDescription { get; set; }
        public string VehicleWeightCode { get; set; }
        public decimal WeightFrom { get; set; }
        public decimal WeightTo { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
    }
}
