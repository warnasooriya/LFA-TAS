using System;

namespace TAS.Services.Entities
{
    public class AdditionalPolicyData
    {
        public virtual Guid Id { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual Guid DataFieldId { get; set; }
        public virtual string DataFieldCode { get; set; }
        public virtual string Value { get; set; }
        public virtual DateTime DateEntered { get; set; }
        public virtual Guid EnteredUser { get; set; }

    }
}
