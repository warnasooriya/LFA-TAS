using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimInvoiceEntryClaimRequestDto
    {
        public  Guid Id { get; set; }
        public  Guid ClaimInvoiceEntryId { get; set; }
        public  Guid ClaimId { get; set; }
    }
}
