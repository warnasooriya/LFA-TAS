using System;

namespace TAS.Services.Entities
{
    public class TireSizeVariantMap
    {
        public virtual Guid Id { get; set; }
        public virtual Decimal SizeFrom { get; set; }
        public virtual Decimal SizeTo { get; set; }
        public virtual int Quantity { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual Guid ModelId { get; set; }
        public virtual Guid VariantId { get; set; }
        public virtual bool IsActive { get; set; }
        //public virtual Decimal OriginalTireDepth { get; set; }
    }
}
