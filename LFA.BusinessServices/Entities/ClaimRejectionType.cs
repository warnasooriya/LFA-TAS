using System;

namespace TAS.Services.Entities
{
    public class ClaimRejectionType
    {
        public virtual Guid Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }
}
