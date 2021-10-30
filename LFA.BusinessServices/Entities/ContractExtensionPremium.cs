using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ContractExtensionPremium
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }

        public virtual Guid WarrentyTypeId { get; set; }
        public virtual Guid ItemStatusId { get; set; }
        public virtual Guid PremiumBasedOnNett { get; set; }
        public virtual Guid PremiumBasedOnGross { get; set; }
        public virtual decimal MinValueNett { get; set; }
        public virtual decimal MinValueGross { get; set; }
        public virtual decimal MaxValueNett { get; set; }
        public virtual decimal MaxValueGross { get; set; }
        public virtual bool IsCustomerAvailableNett { get; set; }
        public virtual bool IsCustomerAvailableGross { get; set; }
        public virtual bool IsClaimLabourChargesFixed { get; set; }
        public virtual decimal NRP { get; set; }
        public virtual decimal Gross { get; set; }
        public virtual decimal ConversionRate { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual Guid CurrencyId { get; set; }
        //public virtual DateTime EntryDateTime { get; set; }

    }
}
