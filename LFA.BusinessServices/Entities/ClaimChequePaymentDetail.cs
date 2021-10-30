using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimChequePaymentDetail
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimChequePaymentId { get; set; }
        public virtual Guid ClaimBatchGroupId { get; set; }
        public virtual DateTime EntryDate { get; set; }
        public virtual Guid EntryBy { get; set; }
    }
}
