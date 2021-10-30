using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    

    public class ClaimEndorsement_
    {
        public Guid claimId { get; set; }
        public Guid policyId { get; set; }
        public Guid loggedInUserId { get; set; }
        public Guid dealerId { get; set; }

        //public List<ClaimEndorsementPart_> claimpart { get; set; }
        //public List<ClaimEndorsementLabourCharge_> claimlabourCharge { get; set; }
        //public List<ClaimEndorsementPart_> sundry { get; set; }
        //public List<ClaimEndorsmentEdit> editclaimItems { get; set; }

        public AssesmentEn assesmentDetails { get; set; }
        public List<ClaimEndorsmentEdit> ClaimList { get; set; }     
        public string status { get; set; }
        public bool isGoodwillClaim { get; set; }


      
    }

    public class AssesmentEn
    {
        public Guid policyId { get; set; }
        public Guid claimId { get; set; }
        public Guid dealerId { get; set; }
        public string engineer { get; set; }
        public string conclution { get; set; }
        public Guid loggedInUserId { get; set; }
    }

    public class ClaimEndorsmentEdit
    {
        public decimal authorizedAmt { get; set; }
        public string comment { get; set; }
        public int discountAmount { get; set; }
        public int discountRate { get; set; }
        public Guid? faultId { get; set; }
        public string faultName { get; set; }
        public int goodWillAmount { get; set; }
        public int goodWillRate { get; set; }
        public bool isDiscountPercentage { get; set; }
        public bool isGoodWillPercentage { get; set; }
        public string itemName { get; set; }
        public string itemNumber { get; set; }
        public string itemType { get; set; }
        public int id { get; set; }
        public int qty { get; set; }        
        public Guid parentId { get; set; }
        public Guid partAreaId { get; set; }
        public Guid? partId { get; set; }
        public int partQty { get; set; }
        public decimal unitPrice { get; set; }
        public string remark { get; set; }
        public Guid? serverId { get; set; }
        public string status { get; set; }
        public string statusCode { get; set; }
        public decimal totalGrossPrice { get; set; }
        public decimal totalPrice { get; set; }         
        public Guid  claimId { get; set; }       
        public Guid? policyId { get; set; } 
        //public Guid  entryBy  { get; set; }                           
         
                                   
    }

    public class ClaimEndorsementPart_
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
    }

    public class ClaimEndorsementLabourCharge_
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
    public class SaveClaimEndorsementProcessRequestDto
    {
        public ClaimEndorsement_ ClaimEndorsementDetails { get; set; }
    }
}
