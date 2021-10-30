using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimEndorseRequestDto
    {
        public Guid claimId { get; set; }
        public string status { get; set; }
        public bool isGoodwillClaim { get; set; }
        public DateTime claimDate { get; set; }
        public decimal claimMileage { get; set; }
        public List<Guid> policyDocIds { get; set; }
        public List<ClaimItem> claimItems { get; set; }
        public CompalintData compalinData { get; set; }
        public Guid loggedInUserId { get; set; }
        
    }

    public class ClaimItem
    {
        public int id { get; set; }
        public Guid? partAreaId { get; set; }
        public Guid? partId { get; set; }
        public string partNumber { get; set; }
        public string partName { get; set; }
        public decimal partQty { get; set; }
        public decimal unitPrice { get; set; }
        public string remark { get; set; }
        public string goodWillType { get; set; }
        public decimal goodWillValue { get; set; }
        public decimal goodWillAmount { get; set; }
        public string discountType { get; set; }
        public decimal discountValue { get; set; }
        public decimal discountAmount { get; set; }
        public Guid? faultId { get; set; }
        public string partDiscountScheme { get; set; }
        public decimal nettAmount { get; set; }
        public decimal authorizedAmount { get; set; }
        public string itemStatus { get; set; }
        public string itemStatusName { get; set; }
        public Guid? serverId { get; set; }
        public string l_chargeType { get; set; }
        public string l_chargeTypeName { get; set; }
        public decimal l_hourlyRate { get; set; }
        public decimal l_hours { get; set; }
        public decimal l_totalAmount { get; set; }
        public string l_description { get; set; }
        public string l_goodWillType { get; set; }
        public decimal l_goodWillValue { get; set; }
        public decimal l_goodWillAmount { get; set; }
        public string l_discountType { get; set; }
        public decimal l_discountValue { get; set; }
        public decimal l_discountAmount { get; set; }
        public string l_labourDiscountScheme { get; set; }
        public decimal l_nettAmount { get; set; }
        public decimal l_authorizedAmount { get; set; }
        public Guid? fault { get; set; }
        public Guid? rejectionTypeId { get; set; }

        
    }

    public class CompalintData
    {
        public string customer { get; set; }
        public string dealer { get; set; }
        public string engineer { get; set; }
        public string conclution { get; set; }
    }

}
