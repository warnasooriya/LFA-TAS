using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class ContractTaxesResponseDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public Guid CountryTaxesId { get; set; }

        public bool IsContractTaxesExists { get; set; }

    }
}
