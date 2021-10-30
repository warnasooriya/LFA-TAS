using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class CurrencyConversionUtilDto
    {
        public Guid currencyPeriodId { get; set; }
        public Guid DealerCurrencyId { get; set; }
        public decimal ConversionRate { get; set; }
        public string currencyCode {get; set; }
    }
}
