using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClamItemRequestDto
    {
        public int id { get; set; }
        public Guid serverId { get; set; }
        public Guid? partAreaId { get; set; }
        public Guid? partId { get; set; }
        public Guid? faultId { get; set; }
        public Guid? rejectionTypeId { get; set; }
        public Guid? discountSchemeId { get; set; }

        public Guid policyId { get; set; }
        public Guid entryBy { get; set; }
        public Guid claimId { get; set; }
        public Guid dealerId { get; set; }
        public Guid causeOfFailureId { get; set; }

        
        public string itemName { get; set; }
        public string itemNumber { get; set; }
        public decimal qty { get; set; }
        public decimal unitPrice { get; set; }
        public decimal totalPrice { get; set; }
        public string itemType { get; set; }
        public string remarks { get; set; }
        public decimal totalGrossPrice { get; set; }
        public decimal discountRate { get; set; }
        public decimal discountAmount { get; set; }
        public bool isDiscountPercentage { get; set; }
        public decimal goodWillRate { get; set; }
        public decimal goodWillAmount { get; set; }
        public bool isGoodWillPercentage { get; set; }
        public int parentId { get; set; }
        public string status { get; set; }
        public decimal authorizedAmt { get; set; }
        public string comment { get; set; }

      
    }
}
