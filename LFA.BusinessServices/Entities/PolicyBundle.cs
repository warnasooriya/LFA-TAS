using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PolicyBundle
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid DealerLocationId { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Guid ExtensionTypeId { get; set; }
        public virtual Decimal Premium { get; set; }
        public virtual Guid PremiumCurrencyTypeId { get; set; }
        public virtual Guid CoverTypeId { get; set; }
        public virtual string HrsUsedAtPolicySale { get; set; }
        public virtual bool IsPreWarrantyCheck { get; set; }
        public virtual DateTime PolicySoldDate { get; set; }
        public virtual Guid SalesPersonId { get; set; }
        public virtual string PolicyNo { get; set; }
        public virtual bool IsSpecialDeal { get; set; }
        public virtual bool IsPartialPayment { get; set; }
        public virtual Decimal DealerPayment { get; set; }
        public virtual Decimal Discount { get; set; }
        public virtual Guid DealerPaymentCurrencyTypeId { get; set; }
        public virtual Decimal CustomerPayment { get; set; }
        public virtual Guid CustomerPaymentCurrencyTypeId { get; set; }
        public virtual Guid PaymentModeId { get; set; }
        public virtual Guid PaymentTypeId { get; set; }

        
        public virtual string RefNo { get; set; }
        public virtual string Comment { get; set; }
        public virtual Guid CustomerId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsPolicyCanceled { get; set; }
        public virtual bool DealerPolicy { get; set; }

        public virtual string BookletNumber { get; set; }
        public virtual Guid ContractInsuaranceLimitationId { get; set; }
        public virtual Guid ContractExtensionsId { get; set; }
        public virtual Guid ContractExtensionPremiumId { get; set; }
        public virtual DateTime MWStartDate { get; set; }
        public virtual bool MWIsAvailable { get; set; }
    }
    public class PolicyBundleInfo : PolicyBundle
    {
        public virtual Guid ItemId { get; set; }
        public virtual string Type { get; set; }
    }
     
}
