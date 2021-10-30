using System;

namespace TAS.Services.Entities
{
    public class BundledProduct
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual Guid ParentProductId { get; set; }
        public virtual bool IsCurrentProduct { get; set; }

    }
}