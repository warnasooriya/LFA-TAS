
using System;

namespace TAS.Services.Entities
{
    public class PolicyTax
    {

        public virtual Guid Id { get; set; }
        public virtual Guid PolicyId { get; set; }
        public virtual Guid TaxTypeId { get; set; }
        public virtual decimal TaxValue { get; set; }
        public virtual bool IsOnPreviousTax { get; set; }
        public virtual bool IsPercentage { get; set; }
        public virtual bool IsOnNRP { get; set; }
        public virtual bool IsOnGross { get; set; }
        public virtual int IndexVal { get; set; }
        public virtual decimal ValueIncluededInPolicy { get; set; }



    }
}
