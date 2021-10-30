using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ConfirmedBordxForGridResponseDto
    {
        public int Id { get; set; }
        public string CommodityType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string Dealer { get; set; }

    }
}
