using System;
using System.Collections.Generic;

namespace TAS.Services.Entities
{
    public class VariantInfo
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual string VariantName { get; set; }
        public virtual int? FromModelYear { get; set; }
        public virtual int? ToModelYear { get; set; }
        public virtual Guid EngineCapacityId { get; set; }
        public virtual Guid CylinderCountId { get; set; }
        public virtual string BodyCode { get; set; }
        public virtual List<Guid> BodyTypes { get; set; }
        public virtual List<Guid> Countrys { get; set; }
        public virtual List<Guid> FuelTypes { get; set; }
        public virtual List<Guid> Aspirations { get; set; }
        public virtual List<Guid> Transmissions { get; set; }
        public virtual List<Guid> DriveTypes { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }
    public class Variant
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual string VariantName { get; set; }
        public virtual int? FromModelYear { get; set; }
        public virtual int? ToModelYear { get; set; }
        public virtual Guid EngineCapacityId { get; set; }
        public virtual Guid CylinderCountId { get; set; }
        public virtual string BodyCode { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual decimal GrossWeight { get; set; }
        //public virtual bool IsForuByFour { get; set; }
    }
    public class VariantBodyTypes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid BodyTypeId { get; set; }
        public virtual Guid VariantId { get; set; }
    }
    public class VariantCountrys
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid VariantId { get; set; }
    }
    public class VariantFuelTypes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid FuelTypeId { get; set; }
        public virtual Guid VariantId { get; set; }
    }
    public class VariantAspirations
    {
        public virtual Guid Id { get; set; }
        public virtual Guid AspirationId { get; set; }
        public virtual Guid VariantId { get; set; }
    }
    public class VariantTransmissions
    {
        public virtual Guid Id { get; set; }
        public virtual Guid TransmissionId { get; set; }
        public virtual Guid VariantId { get; set; }
    }
    public class VariantDriveTypes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DriveTypeId { get; set; }
        public virtual Guid VariantId { get; set; }
    }


}
