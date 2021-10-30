using System;

namespace TAS.DataTransfer.Requests
{
    public class ClaimRejectionRequestDto
    {
        public virtual Guid claimId { get; set; }
        public virtual Guid rejectionTypeId { get; set; }
        public virtual string rejectionComment { get; set; }
        public virtual Guid loggedInUserId { get; set; }

        
    }
}
