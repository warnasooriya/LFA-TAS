using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    public class AllPremiumDetailsByExtensionIdResponseDto
    {
        public Guid warrentyTypeId { get; set; }
        public Guid itemStatusId { get; set; }

        public List<PremiumAddonsNettV2> premiumAddonsNett { get; set; }
        public List<PremiumAddonsGrossV2> premiumAddonsGross { get; set; }
        public decimal nrp { get; set; }
        public decimal gross { get; set; }

        public Guid premiumBasedOnIdNett { get; set; }
        public bool IsMinMaxVisibleNett { get; set; }
        public bool percentageVisibleNett { get; set; }
        public bool isCustAvailableNett { get; set; }
        public decimal minValueNett { get; set; }
        public decimal maxValueNett { get; set; }
        public decimal minValueGross { get; set; }
        public decimal maxValueGross { get; set; }
        public bool manufacturerWarrantyNett { get; set; }
        public Guid premiumBasedOnIdGross { get; set; }
        public bool percentageVisibleGross { get; set; }
        public bool IsMinMaxVisibleGross { get; set; }
        public bool isCustAvailableGross { get; set; }
        public bool manufacturerWarrantyGross { get; set; }

    }
}
