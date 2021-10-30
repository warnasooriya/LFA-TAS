using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class CurrencyConversionRequestDto
    {
        public Guid Id { get; set; }
        public decimal Rate { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid CurrencyConversionPeriodId { get; set; }
        public string Comment { get; set; }

        public bool CurrencyConversionInsertion
        {
            get;
            set;
        }

    }
}
