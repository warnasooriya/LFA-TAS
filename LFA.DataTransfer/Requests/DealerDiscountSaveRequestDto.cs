using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    
    public class DealerDiscountSaveRequestDto
    {
        public Guid id { get; set; }
        public string itemType { get; set; }
        public Guid countryId { get; set; }
        public Guid dealerId { get; set; }
        public Guid makeId { get; set; }
        public Guid discounSchemeId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public decimal discountRate { get; set; }
        public decimal goodwillRate { get; set; }
        public bool isActive { get; set; }
        public Guid userId { get; set; }
    }
}
