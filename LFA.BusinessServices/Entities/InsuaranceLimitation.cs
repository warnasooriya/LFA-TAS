using System;

namespace TAS.Services.Entities
{
    public class InsuaranceLimitation
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual string CommodityTypeCode { get; set; }
        public virtual string InsuaranceLimitationName { get; set; }
        public virtual decimal Km { get; set; }
        public virtual int Months { get; set; }
        public virtual decimal Hrs { get; set; }

        
        public virtual bool TopOfMW { get; set; }
        public virtual bool IsRsa { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryBy { get; set; }
    }
}