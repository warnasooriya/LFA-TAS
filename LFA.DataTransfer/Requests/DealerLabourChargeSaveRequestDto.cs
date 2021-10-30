using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealerLabourChargeSaveRequestDto
    {
        public  Guid Id { get; set; }
        public  Guid DealerId { get; set; }
        public  Guid CountryId { get; set; }
        public  Guid MakeId { get; set; }
        public  List<Guid> ModelId { get; set; }
        public  Guid CurrencyId { get; set; }
        public  Guid CurrencyPeriodId { get; set; }
        public  DateTime StartDate { get; set; }
        public  DateTime EndDate { get; set; }
        public  decimal LabourChargeValue { get; set; }
        public Guid userId { get; set; }
    }
}
