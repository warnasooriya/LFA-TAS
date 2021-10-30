using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class NRPCommissionContractMappingRequestDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public Guid NRPCommissionId { get; set; }
        public Decimal Commission { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsOnNRP { get; set; }
        public bool IsOnGROSS { get; set; }

        public bool NRPCommissionContractMappingInsertion
        {
            get;
            set;
        }

    }
}
