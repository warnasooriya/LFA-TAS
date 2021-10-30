using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimBordxProcess
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimBordxId { get; set; }
        public virtual Guid InsurerId { get; set; }
        public virtual Guid ReinsurerId { get; set; }
        public virtual int Year { get; set; }
        public virtual int Month { get; set; }
        public virtual bool IsProcessed { get; set; }
        public virtual bool IsConfirmed { get; set; }
        public virtual Guid UserId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid ProcessedUserId { get; set; }
        public virtual DateTime ProcessedDateTime { get; set; }
        public virtual Guid ConfirmedUserId { get; set; }
        public virtual DateTime ConfirmedDateTime { get; set; }

    }
}
