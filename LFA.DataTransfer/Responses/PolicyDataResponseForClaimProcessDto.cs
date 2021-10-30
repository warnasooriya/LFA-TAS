using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyDataResponseForClaimProcessDto
    {
        public string Status { get; set; }
        public string PolicyNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerTel { get; set; }
        public string Serial { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string PolicyDealer { get; set; }
        public string ClaimDealer { get; set; }
        public string PolicyCountry { get; set; }
        public List<DealerData> ClaimDealers { get; set; }
        public Guid CommodityCategoryId { get; set; }
        public Guid MakeId { get; set; }
        public Guid ModelId { get; set; }

        public Guid PolicyDealerId { get; set; }
        public Guid PolicyCountryId { get; set; }


    }

    public class DealerData
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string CurrencyCode { get; set; }

    }
}
