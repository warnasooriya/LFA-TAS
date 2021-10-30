using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class SaveClaimEngineerCommentRequestDto
    {
        public Guid policyId { get; set; }
        public Guid claimId { get; set; }
        public Guid dealerId { get; set; }
        public string engineer { get; set; }
        public string conclution { get; set; }
        public Guid loggedInUserId { get; set; }
    }
}
