using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CurrencyConversionResponseDto
    {
        public Guid Id { get; set; }
        public decimal Rate { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid CurrencyConversionPeriodId { get; set; }
        public string Comment { get; set; }

        public bool IsCurrencyConversionExists { get; set; }

    }
}
