using System;

namespace TAS.Services.Entities
{
    public class AdditionalPolicyModelData
    {
        public virtual Guid Id { get; set; }
        public virtual Guid MakeId { get; set; }
        public virtual string ModelName { get; set; }
        public virtual string ModelCode { get; set; }
        public virtual bool IsActive { get; set; }
    }
}
