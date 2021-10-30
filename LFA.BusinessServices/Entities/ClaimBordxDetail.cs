using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimBordxDetail
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimBordxId { get; set; }
        public virtual Guid ClaimId { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsBatching { get; set; }
    }
}
