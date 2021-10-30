using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class CountryTaxesRequestDto
    {
        public Guid Id { get; set; }
        public Guid CountryId { get; set; }
        public Guid TaxTypeId { get; set; }
        public Decimal TaxValue { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsOnPreviousTax { get; set; }
        public bool IsOnNRP { get; set; }
        public bool IsOnGross { get; set; }
        public Decimal MinimumValue { get; set; }
        public int IndexVal { get; set; }
        public Guid currencyPeriodId { get; set; }
        public Guid TpaCurrencyId { get; set; }
        public Decimal ConversionRate { get; set; }


        public bool CountryTaxesInsertion
        {
            get;
            set;
        }

    }
}
