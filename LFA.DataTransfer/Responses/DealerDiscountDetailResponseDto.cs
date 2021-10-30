using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class DealerDiscountDetailResponseDto
    {
        public Guid DealerDiscountId { get; set; }
        public string ItemType { get; set; }
        public bool IsActive { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Guid CountryId { get; set; }
        public Guid DealerId { get; set; }
        public Guid? MakeId { get; set; }
        public Guid SchemeId { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal GoodWillRate { get; set; }



    }
}
