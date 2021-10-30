
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class RSAAnualPremiumResponseDto
    {
        public Guid Id { get; set; }
        public Guid ContractExtensionId { get; set; }
        public int Year { get; set; }
        public Decimal Value { get; set; }

        public bool IsRSAAnualPremiumExists { get; set; }
    }
}
