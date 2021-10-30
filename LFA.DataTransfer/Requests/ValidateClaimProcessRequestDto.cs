using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ValidateClaimProcessRequestDto
    {
        public virtual Guid claimId { get; set; }
        public virtual Guid policyId { get; set; }
        public virtual Guid loggedInUserId { get; set; }
    }
}
