using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class VehicleDetailsRequestDto
    {


        public Guid Id { get; set; }
        public string VINNo { get; set; }
        public Guid MakeId { get; set; }
        public Guid ModelId { get; set; }
        public Guid CategoryId { get; set; }
        public Guid ItemStatusId { get; set; }
        public Guid CylinderCountId { get; set; }
        public Guid BodyTypeId { get; set; }
        public string PlateNo { get; set; }
        public string ModelYear { get; set; }
        public Guid FuelTypeId { get; set; }
        public Guid AspirationId { get; set; }
        public Guid Variant { get; set; }
        public Guid TransmissionId { get; set; }
        public DateTime ItemPurchasedDate { get; set; }
        public Guid EngineCapacityId { get; set; }
        public Guid DriveTypeId { get; set; }
        public decimal VehiclePrice { get; set; }
        public decimal DealerPrice { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public Guid CommodityUsageTypeId { get; set; }
        public Guid CountryId { get; set; }
        public Guid DealerId { get; set; }
        public  Guid currencyPeriodId { get; set; }
        public  Guid DealerCurrencyId { get; set; }
        public  decimal ConversionRate { get; set; }
        public  decimal GrossWeight { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string EngineNumber { get; set; }
        public bool VehicleDetailsInsertion
        {
            get;
            set;
        }

    }
}
