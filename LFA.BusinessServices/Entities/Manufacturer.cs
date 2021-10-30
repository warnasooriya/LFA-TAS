using System;
using System.Collections.Generic;
namespace TAS.Services.Entities
{
    public class Manufacturer
    {
        public virtual Guid Id { get; set; }
        public virtual string ManufacturerCode { get; set; }
        public virtual string ManufacturerName { get; set; }
        public virtual string ManufacturerClassId { get; set; }
        public virtual bool IsWarrentyGiven { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }

    public class ManufacturerComodityTypes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ManufacturerId { get; set; }
        public virtual Guid CommodityTypeId { get; set; }

    }

    public class ManufacturerWithCommodityTypes
    {
        public virtual Guid Id { get; set; }
        public virtual string ManufacturerCode { get; set; }
        public virtual string ManufacturerName { get; set; }
        public virtual List<Guid> ComodityTypes { get; set; }
        public virtual string ManufacturerClassId { get; set; }
        public virtual bool IsWarrentyGiven { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }
}