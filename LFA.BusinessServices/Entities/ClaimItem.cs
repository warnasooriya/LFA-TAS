using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimItem
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimId { get; set; }
        public virtual Guid ClaimItemTypeId { get; set; }
        public virtual Guid? PartId { get; set; }
        public virtual Guid ParentId { get; set; }
        public virtual Guid? FaultId { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual Guid? RejectionTypeId { get; set; }
        public virtual Guid? DiscountSchemeId { get; set; }


        public virtual string DiscountSchemeCode { get; set; }
        public virtual string ItemName { get; set; }
        public virtual string ItemCode { get; set; }
        public virtual decimal UnitPrice { get; set; }
        public virtual decimal Quantity { get; set; }
        public virtual decimal TotalGrossPrice { get; set; }
        public virtual decimal DiscountRate { get; set; }
        public virtual decimal GoodWillRate { get; set; }
        public virtual decimal DiscountAmount { get; set; }
        public virtual decimal GoodWillAmount { get; set; }
        public virtual decimal AuthorizedAmount { get; set; }
        public virtual decimal ConversionRate { get; set; }



        public virtual bool IsDiscountPercentage { get; set; }
        public virtual bool IsGoodWillPercentage { get; set; }
        public virtual decimal TotalPrice { get; set; }
        public virtual string Remark { get; set; }
        public virtual string TpaComment { get; set; }
        public virtual bool? IsApproved { get; set; }
        public virtual long SeqId { get; set; }

        public virtual Guid CauseOfFailureId { get; set; }

    }
}
