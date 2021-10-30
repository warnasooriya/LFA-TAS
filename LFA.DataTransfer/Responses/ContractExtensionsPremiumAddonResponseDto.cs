using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ContractExtensionsPremiumAddonResponseDto
    {
        public Guid Id { get; set; }
        public Guid ContractExtensionId { get; set; }
        public Guid PremiumAddonTypeId { get; set; }
        public decimal Value { get; set; }
        public string PremiumType { get; set; }
        public bool IsContractExtensionsPremiumAddonExists { get; set; }

    }
}
