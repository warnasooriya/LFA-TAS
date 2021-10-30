using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class AddClaimEndorsementDataRequestDto
    {
        public Guid claimId { get; set; }
        public Guid policyId { get; set; }
        public Guid loggedInUserId { get; set; }
        public Guid dealerId { get; set; }
        public List<ClaimEndorsementPartDetailsRequestDto> part { get; set; }
    }

}
