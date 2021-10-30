using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class AddClaimEndorsementItemResponseDto
    {
        public string Status { get; set; }
        public string ClaimNo { get; set; }
        public decimal RequestedAmount { get; set; }
        public decimal AuthorizedAmount { get; set; }
        public Guid ClaimId { get; set; }
        public bool IsReload { get; set; }
    }
}
