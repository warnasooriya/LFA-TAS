using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class FuelType
    {
        public virtual Guid FuelTypeId { get; set; }
        public virtual string FuelTypeCode { get; set; }
        public virtual string FuelTypeDescription { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }
}
