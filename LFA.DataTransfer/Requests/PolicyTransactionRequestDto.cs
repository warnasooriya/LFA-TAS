using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PolicyTransactionRequestDto
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
        public Decimal DealerPayment { get; set; }
        public Guid DealerPaymentCurrencyTypeId { get; set; }
        public Decimal CustomerPayment { get; set; }
        public Guid CustomerPaymentCurrencyTypeId { get; set; }
        public Guid PaymentModeId { get; set; }
        public string RefNo { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public Guid CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int NationalityId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Guid CountryId { get; set; }
        public char Gender { get; set; }
        public string MobileNo { get; set; }
        public string OtherTelNo { get; set; }
        public int CustomerTypeId { get; set; }
        public int UsageTypeId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string IDNo { get; set; }
        public int IDTypeId { get; set; }
        public Guid CityId { get; set; }
        public DateTime? DLIssueDate { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public string BusinessAddress1 { get; set; }
        public string BusinessAddress2 { get; set; }
        public string BusinessAddress3 { get; set; }
        public string BusinessAddress4 { get; set; }
        public string BusinessTelNo { get; set; }
        public byte[] ProfilePicture { get; set; }
        public Guid BAndWId { get; set; }
        public DateTime ItemPurchasedDate { get; set; }
        public Guid MakeId { get; set; }
        public Guid ModelId { get; set; }
        public string SerialNo { get; set; }
        public decimal ItemPrice { get; set; }
        public Guid CategoryId { get; set; }
        public string ModelYear { get; set; }
        public string AddnSerialNo { get; set; }
        public Guid ItemStatusId { get; set; }
        public string InvoiceNo { get; set; }
        public string ModelCode { get; set; }
        public decimal DealerPrice { get; set; }
        public Guid VehicleId { get; set; }
        public string VINNo { get; set; }
        public Guid CylinderCountId { get; set; }
        public Guid BodyTypeId { get; set; }
        public string PlateNo { get; set; }
        public Guid FuelTypeId { get; set; }
        public Guid AspirationId { get; set; }
        public Guid Variant { get; set; }
        public Guid TransmissionId { get; set; }
        public Guid EngineCapacityId { get; set; }
        public Guid DriveTypeId { get; set; }
        public decimal VehiclePrice { get; set; }
        public bool IsRecordActive { get; set; }
        public Guid TransactionTypeId { get; set; }
        public string CancelationComment { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid ModifiedUser { get; set; }
        public Decimal Discount { get; set; }
        public Guid PolicyBundleId { get; set; }
        public Guid CommodityUsageTypeId { get; set; }
        public Decimal TransferFee { get; set; }
        public bool DealerPolicy { get; set; }

        public  DateTime RegistrationDate { get; set; }
        public List<PolicyContractProductRequestDto> ContractProducts { get; set; }

        public bool PolicyInsertion
        {
            get;
            set;
        }

    }
}
