using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class PremiumAddonTypeResponseDto
    {
        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public string Description { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public int IndexNo { get; set; }
        public bool IsApplicableforVariant { get; set; }

        public bool IsPremiumAddonTypeExists { get; set; }

    }
}
