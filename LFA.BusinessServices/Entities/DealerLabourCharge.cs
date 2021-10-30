using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class DealerLabourCharge
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual decimal LabourChargeValue { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual Guid EntryBy { get; set; }
        
    }
}
