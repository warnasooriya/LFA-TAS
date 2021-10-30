using System;

namespace TAS.Services.Entities
{
    public class ContractExtensionVariant
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual Guid VariantId { get; set; }
    }
}
