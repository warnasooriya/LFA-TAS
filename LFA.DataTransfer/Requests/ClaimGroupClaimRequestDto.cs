using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimGroupClaimRequestDto
    {
        public Guid Id { get; set; }
        public Guid ClaimId { get; set; }
        public decimal Amount { get; set; }
        public String Comment { get; set; }
    }
}
