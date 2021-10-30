using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class CountryDealerProductCommoditiesInsuresResponseDto
    {
        public object countries { get; set; }
        public object dealers { get; set; }
        public object products { get; set; }
        public object commodityTypes { get; set; }
        public object insurers { get; set; }
    }
}
