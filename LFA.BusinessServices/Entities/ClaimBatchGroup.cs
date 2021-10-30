using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimBatchGroup
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimBatchId { get; set; }
        public virtual string GroupName { get; set; }
        public virtual bool IsAllocatedForCheque { get; set; }        
        public virtual DateTime EntryDate { get; set; }
        public virtual Guid EntryBy { get; set; }
        public virtual Decimal TotalAmount { get; set; }
        public virtual String Comment { get; set; }
        public virtual bool IsGoodwill { get; set; } 
        
    }
}
