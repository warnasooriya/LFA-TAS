using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class GetPolicyDetailsForViewInClaimRequestDto
    {
        public Guid claimId { get; set; }
        public Guid policyId { get; set; }
       
    }
}
