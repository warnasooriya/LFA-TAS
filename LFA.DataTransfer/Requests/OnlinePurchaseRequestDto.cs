using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class OnlinePurchaseRequestDto
    {
        public Guid StagOnlinePurchaseID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid CustomerID { get; set; }
        public Guid DealerID { get; set; }
        public Guid DealerLocationId { get; set; }
        public string VINNo { get; set; }
        public Guid MakeID { get; set; }
        public Guid ModelID { get; set; }
        public string Status { get; set; }
        public int ModelYear { get; set; }
        public Guid VarientID { get; set; }
        public Guid EngineCapacityId { get; set; }
        public Guid CeylinderCountID { get; set; }
        public Guid FuelTypeID { get; set; }
        public Guid TransmissionID { get; set; }
        public Guid DriveTypeID { get; set; }
        public Guid BodyTypeID { get; set; }
        public Guid AspirationID { get; set; }
        public DateTime VehiclePurchaseDate { get; set; }
        public string PlateNo { get; set; }
        public decimal VehiclePrice { get; set; }
        public Guid ExtensionTypeID { get; set; }
        public int MilageAtPolicySale { get; set; }
        public Guid ItemCategoryID { get; set; }
        public string SerialNo { get; set; }
        public string InvoiceNo { get; set; }
        public string AddnSerialNo { get; set; }
        public decimal ItemPrice { get; set; }
        public int HrsUsedAtPolicySale { get; set; }
        public Guid ContractID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal SelectedPremium { get; set; }
        public Guid VehicleCategoryID { get; set; }

        public bool PolicyInsertion
        {
            get;
            set;
        }

    }
}
