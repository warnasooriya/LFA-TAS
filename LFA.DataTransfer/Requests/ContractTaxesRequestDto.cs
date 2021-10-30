using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ContractTaxesRequestDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public Guid CountryTaxesId { get; set; }

        public bool ContractTaxesInsertion
        {
            get;
            set;
        }

    }
}
