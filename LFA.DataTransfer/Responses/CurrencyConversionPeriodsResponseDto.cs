using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class CurrencyConversionPeriodsResponseDto
    {
        public List<CurrencyConversionPeriodResponseDto> CurrencyConversionPeriods
        {
            get;
            set;
        }
    }
}
