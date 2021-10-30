using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class Eligibility
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual int AgeFrom { get; set; }
        public virtual int AgeTo { get; set; }
        public virtual int MonthsFrom { get; set; }
        public virtual int MonthsTo { get; set; }
        public virtual int MileageFrom { get; set; }
        public virtual int MileageTo { get; set; }
        public virtual Decimal Premium { get; set; }
        public virtual bool IsPercentage { get; set; }
        public virtual string PlusMinus { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual  bool isMandatory { get; set; }
    }
}
