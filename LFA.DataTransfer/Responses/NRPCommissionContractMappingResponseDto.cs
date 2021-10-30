using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class NRPCommissionContractMappingResponseDto
    {
        public Guid Id { get; set; }
        public Guid ContractId { get; set; }
        public Guid NRPCommissionId { get; set; }
        public Decimal Commission { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsOnNRP { get; set; }
        public bool IsOnGROSS { get; set; }

        public bool IsNRPCommissionContractMappingExists { get; set; }

    }
}
