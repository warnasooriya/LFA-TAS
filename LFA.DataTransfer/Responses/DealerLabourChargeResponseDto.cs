using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class DealerLabourChargeResponseDto
    {
        public Guid DealerLabourChargeId { get; set; }
        public Guid DealerId { get; set; }
        public Guid CountryId { get; set; }
        public Guid MakeId { get; set; }
        public List<Guid> ModelId { get; set; }
        public Guid CurrencyId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public decimal LabourChargeValue { get; set; }
    }
}
