using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class VehicleWeight
    {
        public virtual Guid Id { get; set; }
        public virtual string VehicleWeightDescription { get; set; }
        public virtual string VehicleWeightCode { get; set; }
        public virtual decimal WeightFrom { get; set; }
        public virtual decimal WeightTo { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }
}
