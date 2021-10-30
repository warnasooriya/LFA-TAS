using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PartWithClaimRequestDto
    {
        public Guid claimId { get; set; }
        public Guid policyId { get; set; }
        public Guid loggedInUserId { get; set; }
        public Guid dealerId { get; set; }
        public LabourDetails labour { get; set; }
        public PartDetails part { get; set; }
        public bool isUpdate { get; set; }
    }
    public class LabourDetails
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
        public string labourDiscountScheme { get; set; }
        public decimal nettAmount { get; set; }
        public decimal authorizedAmount { get; set; }
  
        
    }
    public class PartDetails
    {
        public int id { get; set; }
        public Guid partAreaId { get; set; }
        public Guid partId { get; set; }
        public string partNumber { get; set; }
        public string partName { get; set; }
        public int partQty { get; set; }
        public decimal unitPrice { get; set; }
        public string remark { get; set; }
        public bool isRelatedPartsAvailable { get; set; }
        public int allocatedHours { get; set; }
        public string goodWillType { get; set; }
        public int goodWillValue { get; set; }
        public int goodWillAmount { get; set; }
        public string discountType { get; set; }
        public int discountValue { get; set; }
        public int discountAmount { get; set; }
        public Guid fault { get; set; }
        public decimal nettAmount { get; set; }
        public decimal authorizedAmount { get; set; }
        public string itemStatus { get; set; }
        public Guid? rejectionTypeId { get; set; }
        public Guid causeOfFailureId { get; set; }

        public Guid serverId { get; set; }

        public string partDiscountScheme { get; set; }

        
        
    }
}
