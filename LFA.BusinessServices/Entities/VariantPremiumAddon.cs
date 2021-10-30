using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class VariantPremiumAddon
    {
        public virtual Guid Id { get; set; }
        public virtual Guid VariantId { get; set; }
        public virtual Guid PremiumAddonTypeId { get; set; }
    }
}
