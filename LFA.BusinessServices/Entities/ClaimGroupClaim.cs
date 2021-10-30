using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimGroupClaim
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimGroupId { get; set; }
        public virtual Guid ClaimId { get; set; }        
        public virtual DateTime EntryDate { get; set; }
        public virtual Guid EntryBy { get; set; }
        public virtual Decimal Amount { get; set; }
        public virtual String Comment { get; set; }
    }
}
