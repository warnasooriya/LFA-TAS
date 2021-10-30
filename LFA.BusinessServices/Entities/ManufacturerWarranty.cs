using System;

namespace TAS.Services.Entities
{
    public class ManufacturerWarranty
    {
        public virtual Guid Id { get; set; }
        public virtual Guid MakeId { get; set; }

        public virtual string WarrantyName { get; set; }
        public virtual int WarrantyMonths { get; set; }
        public virtual int WarrantyKm { get; set; }
        public virtual DateTime ApplicableFrom { get; set; }
        //public virtual DateTime ApplicableTo { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual bool IsUnlimited { get; set; }
    }

  
}