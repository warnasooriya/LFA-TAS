using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimSubmissionRequestDto
    {
        public List<ClaimItemList> claimItemList { get; set; }
        public decimal totalClaimAmount { get; set; }
        public Guid requestedUserId { get; set; }
        public Guid dealerId { get; set; }
        public Guid policyId { get; set; }
        public Complaint complaint { get; set; }
        public List<Guid> attachmentIds { get; set; }
        public decimal claimMileage { get; set; }
        public DateTime claimDate { get; set; }
        public DealerEntry policyDetails { get; set; }
        public string commentDealer { get; set; }
       // public ClaimSubmissionDetails ClaimSubmissionDetails { get; set; }

    }

    public class ClaimItemList
    {
        public int id { get; set; }
        public Guid serverId { get; set; }
        public Guid partAreaId { get; set; }
        public Guid? partId { get; set; }
        public Guid? faultId { get; set; }
        public Guid? rejectionTypeId { get; set; }
        public Guid? discountSchemeId { get; set; }
        public Guid? causeOfFailureId { get; set; }

        public string discountSchemeCode { get; set; }
        public string faultName { get; set; }

        public string itemName { get; set; }
        public string itemNumber { get; set; }
        public decimal qty { get; set; }
        public decimal unitPrice { get; set; }
        public decimal totalPrice { get; set; }
        public decimal authorizedAmount { get; set; }

        public string itemType { get; set; }
        public string remarks { get; set; }

        public decimal totalGrossPrice { get; set; }
        public decimal discountRate { get; set; }
        public decimal discountAmount { get; set; }
        public bool isDiscountPercentage { get; set; }
        public decimal goodWillRate { get; set; }
        public decimal goodWillAmount { get; set; }
        public bool isGoodWillPercentage { get; set; }
        public Guid parentId { get; set; }
        public string status { get; set; }
        public string statusCode { get; set; }
        public string comment { get; set; }

        public decimal UnUsedTireDepth { get; set; }




    }

    public class Complaint
    {
        public string customer { get; set; }
        public string dealer { get; set; }
        public string engineer { get; set; }
        public string conclution { get; set; }

    }

    public class DealerEntry
    {
        public string policyNo { get; set; }
        public string vinNo { get; set; }
        public string plateNumber { get; set; }
        public string customerName { get; set; }
        public string mobileNo { get; set; }
        public decimal failureMileage { get; set; }
        public decimal lastServiceMileage { get; set; }
        public DateTime lastServiceDate { get; set; }
        public DateTime failureDate { get; set; }


        public Guid makeId { get; set; }
        public Guid modelId { get; set; }


    }

    public class ClaimSubmissionDetails
    {
        public DateTime failureDate { get; set; }
        public decimal failureMileage { get; set; }
        public string customerName { get; set; }
        public decimal lastServiceMileage { get; set; }
        public DateTime lastServiceDate { get; set; }
    }
}
