using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxNumbersRequestDto
    {
        public Guid CommodityTypeId { get; set; }
        public Guid InsurerId { get; set; }
        public Guid ReinsurerId { get; set; }
        public Guid ProductId { get; set; }

        public int year { get; set; }
        public int month { get; set; }

    }
}
