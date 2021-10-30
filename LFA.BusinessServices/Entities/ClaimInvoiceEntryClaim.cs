using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimInvoiceEntryClaim
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimInvoiceEntryId { get; set; }
        public virtual Guid ClaimId { get; set; }
    }
}
