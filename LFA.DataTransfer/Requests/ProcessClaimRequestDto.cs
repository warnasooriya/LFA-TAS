using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ProcessClaimRequestDto
    {
        public Guid claimId { get; set; }
        public string status { get; set; }
        public DateTime claimDate { get; set; }
        public decimal claimMileage { get; set; }
        public List<Guid> policyDocIds { get; set; }
        public bool isGoodwillClaim { get; set; }
        public string comment { get; set; }
        public Guid loggedInUserId { get; set; }
        public List<Guid> claimtaxIds { get; set; }
        public decimal athorizedAmountbefortax { get; set; }
        public decimal athorizedAmount { get; set; }


    }
}
