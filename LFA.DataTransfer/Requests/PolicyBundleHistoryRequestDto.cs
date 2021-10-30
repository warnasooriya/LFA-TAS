using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PolicyBundleHistoryRequestDto
    {
        public Guid Id { get; set; }
        public Guid PolicyId { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid ProductId { get; set; }
        public Guid DealerId { get; set; }
        public Guid DealerLocationId { get; set; }
        public Guid ContractId { get; set; }
        public Guid ExtensionTypeId { get; set; }
        public Decimal Premium { get; set; }
        public Guid PremiumCurrencyTypeId { get; set; }
        public Guid CoverTypeId { get; set; }
        public string HrsUsedAtPolicySale { get; set; }
        public bool IsPreWarrantyCheck { get; set; }
        public DateTime PolicySoldDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public Guid SalesPersonId { get; set; }
        public string PolicyNo { get; set; }
        public bool IsSpecialDeal { get; set; }
        public bool IsPartialPayment { get; set; }
        public bool IsApproved { get; set; }
        public Decimal DealerPayment { get; set; }
        public Decimal Discount { get; set; }
        public Guid DealerPaymentCurrencyTypeId { get; set; }
        public Decimal CustomerPayment { get; set; }
        public Guid CustomerPaymentCurrencyTypeId { get; set; }
        public Guid PaymentModeId { get; set; }
        public string RefNo { get; set; }
        public string Comment { get; set; }
        public Guid ItemId { get; set; }
        public string Type { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public bool IsPolicyCanceled { get; set; }
        public List<PolicyRequestDto> PolicyBundle { get; set; }
        public Guid TransactionTypeId { get; set; }
        public bool DealerPolicy { get; set; }

        public bool PolicyBundleInsertion
        {
            get;
            set;
        }

    }
}
