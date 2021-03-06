using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimCriteria
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual string Label { get; set; }
        public virtual decimal Presentage { get; set; }
        public virtual int Year { get; set; }

    }
}
