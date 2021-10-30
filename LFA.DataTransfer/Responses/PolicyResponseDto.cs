using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class PolicyResponseDto
    {
        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductCode { get; set; }
        public Guid DealerId { get; set; }
        public Guid DealerLocationId { get; set; }
        public Guid tpaBranchId { get; set; }

        public Guid ContractId { get; set; }
        public Guid ExtensionTypeId { get; set; }
        public Decimal Premium { get; set; }
        public Guid PremiumCurrencyTypeId { get; set; }
        public Guid DealerPaymentCurrencyTypeId { get; set; }
        public Guid CustomerPaymentCurrencyTypeId { get; set; }
        public Guid CoverTypeId { get; set; }
        public string HrsUsedAtPolicySale { get; set; }
        public bool IsPreWarrantyCheck { get; set; }
        public bool IsApproved { get; set; }
        public DateTime ApprovedDate { get; set; }
        public bool IsRenewed { get; set; }
        public string Status { get; set; }
        public DateTime PolicySoldDate { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public Guid SalesPersonId { get; set; }
        public string PolicyNo { get; set; }
        public bool IsSpecialDeal { get; set; }
        public bool IsPartialPayment { get; set; }
        public Decimal DealerPayment { get; set; }
        public Decimal Discount { get; set; }
        public Decimal DiscountPercentage { get; set; }
        public Decimal CustomerPayment { get; set; }
        public Guid PaymentModeId { get; set; }
        public Guid PaymentTypeId { get; set; }
        public string RefNo { get; set; }
        public string Comment { get; set; }
        public Guid ItemId { get; set; }
        public string Type { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public bool IsPolicyCanceled { get; set; }
        public Guid PolicyBundleId { get; set; }
        public Decimal TransferFee { get; set; }
        public Guid BordxId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string ForwardComment { get; set; }
        public bool DealerPolicy { get; set; }
        public string BookletNumber { get; set; }
        public  Guid ContractInsuaranceLimitationId { get; set; }
        public  Guid ContractExtensionsId { get; set; }
        public  Guid ContractExtensionPremiumId { get; set; }

        public DateTime MWEnddate { get; set; }
        public DateTime MWStartDate { get; set; }
        public bool MWIsAvailable { get; set; }
        public string MWKM { get; set; }
        public int MWMonths { get; set; }
        public int ExtMonths { get; set; }
        public decimal ExtKM { get; set; }
        public DateTime ExtStartDate { get; set; }
        public DateTime ExtEndDate { get; set; }
        public int KMCutOff { get; set; }
        public string jwt { get; set; }
        public Guid CurrencyPeriodId { get; set; }
        public bool IsBulkUpload { get; set; }

        public List<PolicyContractProductResponseDto> ContractProducts { get; set; }

        public List<PolicyContractTireProductResponseDto> ContractTireProducts { get; set; }

        public bool IsPolicyExists { get; set; }

        public bool IsEndorsementApprovalPending { get; set; }
        public List<Guid> UploadAttachments { get; set; }
    }
}
