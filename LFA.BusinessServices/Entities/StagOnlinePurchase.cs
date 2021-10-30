using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class StagOnlinePurchase
    {
        public virtual Guid StagOnlinePurchaseID { get; set; }
        public virtual DateTime PurchaseDate { get; set; }
        public virtual Guid CustomerID { get; set; }
        public virtual Guid DealerID { get; set; }
        public virtual Guid DealerLocationId { get; set; }
        public virtual string VINNo { get; set; }
        public virtual Guid MakeID { get; set; }
        public virtual Guid ModelID { get; set; }
        public virtual string Status { get; set; }
        public virtual int ModelYear { get; set; }
        public virtual Guid VarientID { get; set; }
        public virtual Guid EngineCapacityId { get; set; }
        public virtual Guid CeylinderCountID { get; set; }
        public virtual Guid FuelTypeID { get; set; }
        public virtual Guid TransmissionID { get; set; }
        public virtual Guid DriveTypeID { get; set; }
        public virtual Guid BodyTypeID { get; set; }
        public virtual Guid AspirationID { get; set; }
        public virtual DateTime VehiclePurchaseDate { get; set; }
        public virtual string PlateNo { get; set; }
        public virtual decimal VehiclePrice { get; set; }
        public virtual Guid ExtensionTypeID { get; set; }
        public virtual int MilageAtPolicySale { get; set; }
        public virtual Guid ItemCategoryID { get; set; }
        public virtual string SerialNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string AddnSerialNo { get; set; }
        public virtual decimal ItemPrice { get; set; }
        public virtual int HrsUsedAtPolicySale { get; set; }
        public virtual Guid ContractID { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual decimal SelectedPremium { get; set; }
        public virtual Guid VehicleCategoryID { get; set; }
        //public virtual Guid VehicleCategoryID { get; set; }
        //public virtual Decimal Premium { get; set; }
        //public virtual Guid PremiumCurrencyTypeId { get; set; }
        //public virtual Guid CoverTypeId { get; set; }
        //public virtual string HrsUsedAtPolicySale { get; set; }
        //public virtual bool IsPreWarrantyCheck { get; set; }
        //public virtual DateTime PolicySoldDate { get; set; }
        //public virtual DateTime PolicyStartDate { get; set; }
        //public virtual DateTime PolicyEndDate { get; set; }
        //public virtual Guid SalesPersonId { get; set; }
        //public virtual string PolicyNo { get; set; }
        //public virtual bool IsSpecialDeal { get; set; }
        //public virtual bool IsPartialPayment { get; set; }
        //public virtual Decimal DealerPayment { get; set; }
        //public virtual Decimal Discount { get; set; }
        //public virtual Guid DealerPaymentCurrencyTypeId { get; set; }
        //public virtual Decimal CustomerPayment { get; set; }
        //public virtual Guid CustomerPaymentCurrencyTypeId { get; set; }
        //public virtual Guid PaymentModeId { get; set; }
        //public virtual string RefNo { get; set; }
        //public virtual string Comment { get; set; }
        //public virtual Guid CustomerId { get; set; }
        //public virtual DateTime EntryDateTime { get; set; }
        //public virtual Guid EntryUser { get; set; }
        //public virtual bool IsApproved { get; set; }
        //public virtual bool IsPolicyCanceled { get; set; }

    }


    
     
}
