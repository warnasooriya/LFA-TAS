using System;

namespace TAS.Services.Entities
{
    public class ManufacturerWarrantyDetails
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ManufacturerWarrantyId { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual Guid CountryId { get; set; }
    }

    public class ManufacturerWarrantyDetailsCInfor
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ManufacturerWarrantyId { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid ModelId { get; set; }
    }
}