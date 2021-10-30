using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Model
    {
        public virtual Guid Id { get; set; }
        public virtual string ModelCode { get; set; }
        public virtual string ModelName { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual Guid CategoryId { get; set; }
        public virtual int NoOfDaysToRiskStart { get; set; }
        public virtual bool WarantyGiven { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Guid ContryOfOrigineId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual bool AdditionalPremium { get; set; }

    }
}
