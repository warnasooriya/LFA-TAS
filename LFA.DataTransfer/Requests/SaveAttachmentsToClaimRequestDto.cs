using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class SaveAttachmentsToClaimRequestDto
    {
        public Guid claimId { get; set; }
        public List<Guid> docIds { get; set; }

    }
}
