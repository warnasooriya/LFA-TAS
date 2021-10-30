using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class AddLabourChargeToClaimEndorsementRequestDto
    {
        public Guid claimId { get; set; }
        public ClaimEndorsementLabourChargeData labourCharge { get; set; }
        public Guid loggedInUserId { get; set; }
        public Guid dealerId { get; set; }
    }

    public class ClaimEndorsementLabourChargeData
    {
        public string chargeType { get; set; }
        public decimal hourlyRate { get; set; }
        public decimal hours { get; set; }
        public decimal totalAmount { get; set; }
        public string description { get; set; }
        public Guid partId { get; set; }
        public string goodWillType { get; set; }
        public decimal goodWillValue { get; set; }
        public decimal goodWillAmount { get; set; }
        public string discountType { get; set; }
        public decimal discountValue { get; set; }
        public decimal discountAmount { get; set; }
    }
}
