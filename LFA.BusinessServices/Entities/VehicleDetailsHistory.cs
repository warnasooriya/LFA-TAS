using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class VehicleDetailsHistory
    {
        public virtual Guid Id { get; set; }
        public virtual Guid VehicleDetailsId { get; set; }
        public virtual string VINNo { get; set; }
        public virtual Guid MakeId { get; set; }//
        public virtual Guid ModelId { get; set; }//
        public virtual Guid CategoryId { get; set; }//
        public virtual Guid ItemStatusId { get; set; }//
        public virtual Guid CylinderCountId { get; set; }
        public virtual Guid BodyTypeId { get; set; }
        public virtual string PlateNo { get; set; }
        public virtual string ModelYear { get; set; }//
        public virtual Guid FuelTypeId { get; set; }
        public virtual Guid AspirationId { get; set; }
        public virtual Guid Variant { get; set; }
        public virtual Guid TransmissionId { get; set; }
        public virtual DateTime ItemPurchasedDate { get; set; }
        public virtual Guid EngineCapacityId { get; set; }
        public virtual Guid DriveTypeId { get; set; }
        public virtual decimal VehiclePrice { get; set; }
        public virtual decimal DealerPrice { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid CommodityUsageTypeId { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual Guid PolicyTransactionId { get; set; }
        public virtual DateTime RegistrationDate { get; set; }
        public virtual decimal GrossWeight { get; set; }
    }
    
}
