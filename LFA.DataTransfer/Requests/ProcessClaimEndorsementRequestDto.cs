using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ProcessClaimEndorsementRequestDto
    {
        public Guid claimId { get; set; }
        public string status { get; set; }
        public bool isGoodwillClaim { get; set; }
    }
}
