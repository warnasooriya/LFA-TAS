using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ConfirmedClaimBordxForGridResponseDto
    {
        public int Id { get; set; }
        public string CommodityType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string Dealer { get; set; }
    }
}
