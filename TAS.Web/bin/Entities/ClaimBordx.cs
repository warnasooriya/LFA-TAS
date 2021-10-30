using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimBordx
    {
        public virtual Guid Id { get; set; }
        public virtual Guid Insurer { get; set; }
        public virtual Guid Reinsurer { get; set; }
        public virtual int BordxYear { get; set; }
        public virtual int Bordxmonth { get; set; }
        public virtual string BordxNumber { get; set; }
        public virtual DateTime Fromdate { get; set; }
        public virtual DateTime Todate { get; set; }
        public virtual bool IsProcessed { get; set; }
        public virtual bool IsConfirmed { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid ProcessedUserId { get; set; }
        public virtual DateTime ProcessedDateTime { get; set; }
        public virtual Guid ConfirmedUserId { get; set; }
        public virtual DateTime ConfirmedDateTime { get; set; }
        public virtual bool IsPaid { get; set; }
    }
}
