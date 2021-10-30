using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealerDiscountForClaimRequestDto
    {
        public Guid claimId { get; set; }
        public Guid policyId { get; set; }
        public string type { get; set; }
        public Guid schemeId { get; set; }
        public Guid makeId { get; set; }
        public Guid dealerId { get; set; }
        public Guid countryId { get; set; }

    }
}
