using System;

namespace TAS.DataTransfer.Requests
{
    public class ClaimInformationRequestDto
    {
        public Guid claimId { get; set; }
        public string informationMsg { get; set; }
        public Guid loggedInUserId { get; set; }

        
    }
}
