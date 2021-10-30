using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class TyreContractRequestDto
    {
        public Guid CommodityTypeId { get; set; }
        public Guid countryId { get; set; }
        public Guid dealerId { get; set; }
        public string CommodityUsageTypeId { get; set; }
        public List<TyreData> tyreData { get; set; }
        public DateTime purchaseDate { get; set; }

    }
    public class TyreData
    {
        public string position { get; set; }
        public int diameter { get; set; }
        public int quantity { get; set; }
    }

}
