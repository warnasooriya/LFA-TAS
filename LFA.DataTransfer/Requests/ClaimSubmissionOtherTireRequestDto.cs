using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimSubmissionOtherTireRequestDto
    {
        public List<ClaimItemList> claimItemList { get; set; }
        public Guid requestedUserId { get; set; }
        public Guid dealerId { get; set; }
        public Guid Id { get; set; }
        public Guid policyId { get; set; }
        public List<Guid> attachmentIds { get; set; }
        public DateTime claimDate { get; set; }
        public DealerEntry policyDetails { get; set; }
        public DealerInvoiceTireDetail OtherTireDetails { get; set; }
        public Guid InvoiceCodeId { get; set; }
        public otherTireUpdateDetails otherTireUpdateDetails { get; set; }
        public decimal totalClaimAmount { get; set; }
        public bool Reject { get; set; }
        public List<ClaimSubmitTyreDetails> tyreDetails { get; set; }
    }


    public class otherTireUpdateDetails
    {
        public frontLeft frontLeft { get; set; }
        public backLeft backLeft { get; set; }
        public frontRight frontRight { get; set; }
        public backRight backRight { get; set; }
    }

    public class frontLeft
    {
        [JsonProperty(PropertyName = "claimItemId")]
        public string claimItemId { get; set; }
        [JsonProperty(PropertyName = "serial")]
        public string serial { get; set; }
        [JsonProperty(PropertyName = "unusedTyreDepth")]
        public string unusedTyreDepth { get; set; }
    }

    public class backLeft
    {

        [JsonProperty(PropertyName = "claimItemId")]
        public string claimItemId { get; set; }
        [JsonProperty(PropertyName = "serial")]
        public string serial { get; set; }
        [JsonProperty(PropertyName = "unusedTyreDepth")]
        public string unusedTyreDepth { get; set; }
    }

    public class frontRight
    {

        [JsonProperty(PropertyName = "claimItemId")]
        public string claimItemId { get; set; }
        [JsonProperty(PropertyName = "serial")]
        public string serial { get; set; }
        [JsonProperty(PropertyName = "unusedTyreDepth")]
        public string unusedTyreDepth { get; set; }
    }

    public class backRight
    {
        [JsonProperty(PropertyName = "claimItemId")]
        public string claimItemId { get; set; }
        [JsonProperty(PropertyName = "serial")]
        public string serial { get; set; }
        [JsonProperty(PropertyName = "unusedTyreDepth")]
        public string unusedTyreDepth { get; set; }
    }

    public class OtherDealerEntry
    {
        public string policyNo { get; set; }
        public string vinNo { get; set; }
        public string plateNumber { get; set; }
        public string customerName { get; set; }
        public decimal failureMileage { get; set; }
        public DateTime failureDate { get; set; }
        public string lastServiceMileage { get; set; }
        public DateTime lastServiceDate { get; set; }

        public Guid makeId { get; set; }
        public Guid modelId { get; set; }
    }

    public class DealerInvoiceTireDetail
    {
        public string customerComplaint { get; set; }
        public string dealerComment { get; set; }
        public string serialFrontRight { get; set;}
        public string unusedTyreDepthFrontRight { get; set;}
        public string serialBackRight { get; set;}
        public string unusedTyreDepthBackRight { get; set;}
        public string serialBackLeft { get; set;}
        public string unusedTyreDepthBackLeft { get; set;}
        public string seriaFrontlLeft { get; set; }
        public string unusedTyreDepthFrontLeft { get; set; }
        public bool frontPositionDisabled { get; set; }
        public bool backPositionDisabled { get; set; }

        public string ErrorMges { get; set; }


    }

    public class UserEnterdTiredetais
    {
        public string serialFrontRight { get; set; }
        public string unusedTyreDepthFrontRight { get; set; }
        public string serialBackRight { get; set; }
        public string unusedTyreDepthBackRight { get; set; }
        public string serialBackLeft { get; set; }
        public string unusedTyreDepthBackLeft { get; set; }
        public string seriaFrontlLeft { get; set; }
        public string unusedTyreDepthFrontLeft { get; set; }
    }

    public class ClaimSubmitTyreDetails {
        public string Position;
        public string ArticleNo;
        public string UnUsedTireDepth;

    }
}
