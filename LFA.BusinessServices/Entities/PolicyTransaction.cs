using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PolicyTransaction
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PolicyBundleTransactionId { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid DealerLocationId { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Guid ExtensionTypeId { get; set; }
        public virtual DateTime RegistrationDate { get; set; }

        
        public virtual Decimal Premium { get; set; }
        public virtual Guid PremiumCurrencyTypeId { get; set; }
        public virtual Guid CoverTypeId { get; set; }
        public virtual string HrsUsedAtPolicySale { get; set; }
        public virtual bool IsPreWarrantyCheck { get; set; }
        public virtual DateTime PolicySoldDate { get; set; }
        public virtual DateTime PolicyStartDate { get; set; }
        public virtual DateTime PolicyEndDate { get; set; }
        public virtual Guid SalesPersonId { get; set; }
        public virtual string PolicyNo { get; set; }
        public virtual bool IsSpecialDeal { get; set; }
        public virtual bool IsPartialPayment { get; set; }
        public virtual Decimal DealerPayment { get; set; }
        public virtual Guid DealerPaymentCurrencyTypeId { get; set; }
        public virtual Decimal CustomerPayment { get; set; }
        public virtual Guid CustomerPaymentCurrencyTypeId { get; set; }
        public virtual Guid PaymentModeId { get; set; }
        public virtual string RefNo { get; set; }
        public virtual string Comment { get; set; }
        public virtual bool IsApproved { get; set; }
        public virtual bool IsRejected { get; set; }
        public virtual Guid ApprovedRejectedBy { get; set; }

        public virtual Guid CustomerId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual int NationalityId { get; set; }
        public virtual DateTime? DateOfBirth { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual char Gender { get; set; }
        public virtual string MobileNo { get; set; }
        public virtual string OtherTelNo { get; set; }
        public virtual int CustomerTypeId { get; set; }
        public virtual int UsageTypeId { get; set; }
        public virtual string Address1 { get; set; }
        public virtual string Address2 { get; set; }
        public virtual string Address3 { get; set; }
        public virtual string Address4 { get; set; }
        public virtual string IDNo { get; set; }
        public virtual int IDTypeId { get; set; }
        public virtual Guid CityId { get; set; }
        public virtual DateTime? DLIssueDate { get; set; }
        public virtual string Email { get; set; }
        public virtual string BusinessName { get; set; }
        public virtual string BusinessAddress1 { get; set; }
        public virtual string BusinessAddress2 { get; set; }
        public virtual string BusinessAddress3 { get; set; }
        public virtual string BusinessAddress4 { get; set; }
        public virtual string BusinessTelNo { get; set; }
        public virtual byte[] ProfilePicture { get; set; }
        public virtual Guid BAndWId { get; set; }
        public virtual DateTime ItemPurchasedDate { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual string SerialNo { get; set; }
        public virtual decimal ItemPrice { get; set; }
        public virtual Guid CategoryId { get; set; }
        public virtual string ModelYear { get; set; }
        public virtual string AddnSerialNo { get; set; }
        public virtual Guid ItemStatusId { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string ModelCode { get; set; }
        public virtual decimal DealerPrice { get; set; }
        public virtual decimal Discount { get; set; }
        public virtual Guid VehicleId { get; set; }
        public virtual string VINNo { get; set; }
        public virtual Guid CylinderCountId { get; set; }
        public virtual Guid BodyTypeId { get; set; }
        public virtual string PlateNo { get; set; }
        public virtual Guid FuelTypeId { get; set; }
        public virtual Guid AspirationId { get; set; }
        public virtual Guid Variant { get; set; }
        public virtual Guid TransmissionId { get; set; }
        public virtual Guid EngineCapacityId { get; set; }
        public virtual Guid DriveTypeId { get; set; }
        public virtual decimal VehiclePrice { get; set; }
        public virtual bool IsRecordActive { get; set; }
        public virtual Guid TransactionTypeId { get; set; }
        public virtual string CancelationComment { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual Guid ModifiedUser { get; set; }
        public virtual Guid PolicyBundleId { get; set; }
        public virtual Guid CommodityUsageTypeId { get; set; }
        public virtual Decimal TransferFee { get; set; }
        public virtual bool DealerPolicy { get; set; }

        public virtual string BookletNumber { get; set; }
        public virtual Guid ContractInsuaranceLimitationId { get; set; }
        public virtual Guid ContractExtensionsId { get; set; }
        public virtual Guid ContractExtensionPremiumId { get; set; }
        public virtual DateTime MWStartDate { get; set; }
        public virtual bool MWIsAvailable { get; set; }
        public virtual string EngineNumber { get; set; }
        //  public virtual DateTime RegistrationDate { get; set; }


    }
}
