using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBordxRequestDto
    {
        public virtual Guid Id { get; set; }
        public virtual Guid Insurer { get; set; }
        public virtual Guid Reinsurer { get; set; }
        public virtual int BordxYear { get; set; }
        public virtual int Bordxmonth { get; set; }
        public virtual string BordxNumber { get; set; }
        public virtual DateTime Fromdate { get; set; }
        public virtual DateTime Todate { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsConformed { get; set; }
        public Guid UserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid ProcessedUserId { get; set; }
        public DateTime ProcessedDateTime { get; set; }
        public Guid ConfirmedUserId { get; set; }
        public DateTime ConfirmedDateTime { get; set; }

        public bool ClaimBordxInsertion
        {
            get;
            set;
        }

    }
}
