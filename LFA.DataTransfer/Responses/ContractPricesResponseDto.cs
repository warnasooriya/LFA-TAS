using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ContractPricesResponseDto
    {
        public List<ContractPriceResponseDto> ContractPrices
        {
            get;
            set;
        }
    }
}
