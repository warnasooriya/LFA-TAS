using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class CountryTaxessResponseDto
    {
        public List<CountryTaxesResponseDto> CountryTaxes
        {
            get;
            set;
        }
    }
}
