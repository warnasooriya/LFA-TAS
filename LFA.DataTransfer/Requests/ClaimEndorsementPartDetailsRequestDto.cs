using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimEndorsementPartDetailsRequestDto
    {
        public int id { get; set; }
        public Guid partAreaId { get; set; }
        public Guid? partId { get; set; }
        public string partNumber { get; set; }
        public string partName { get; set; }
        public int partQty { get; set; }
        public int unitPrice { get; set; }
        public string remark { get; set; }
        public bool isRelatedPartsAvailable { get; set; }
        public int allocatedHours { get; set; }
        public string goodWillType { get; set; }
        public int goodWillValue { get; set; }
        public int goodWillAmount { get; set; }
        public string discountType { get; set; }
        public int discountValue { get; set; }
        public int discountAmount { get; set; }
        public Guid? fault { get; set; }

        public string itemCode { get; set; }

        public Guid claimId { get; set; }
        public Guid policyId { get; set; }
        public Guid loggedInUserId { get; set; }
        public Guid dealerId { get; set; }
        //public Guid claimId { get; set; }

    }
}
