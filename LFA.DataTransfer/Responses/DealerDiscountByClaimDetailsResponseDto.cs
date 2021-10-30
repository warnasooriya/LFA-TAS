using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class DealerDiscountByClaimDetailsResponseDto
    {
        public string Status { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal GoodWillRate { get; set; }


    }
}
