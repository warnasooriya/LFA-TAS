using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class SubmittedClaimResponseDto
    {
        public virtual Guid ClaimId { get; set; }
        public virtual string ClaimNumber { get; set; }
        public virtual string PolicyNumber { get; set; }
        public virtual string CommodityType { get; set; }
        public virtual string CommodityCategory { get; set; }
        public virtual string Make { get; set; }
        public virtual string Model { get; set; }
        public virtual string PolicyDealer { get; set; }
        public virtual string ClaimDealer { get; set; }
        public virtual string Currency { get; set; }
        public virtual string SubmittedBy { get; set; }
        public virtual string TotalClaimAmount { get; set; }
        public virtual string Status { get; set; }
        public virtual string EntryDate { get; set; }

    }
}
