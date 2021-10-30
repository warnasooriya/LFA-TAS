using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class CurrencyConversionPeriodRequestDto
    {
        public Guid Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; }

        public bool CurrencyConversionPeriodInsertion
        {
            get;
            set;
        }

    }
}
