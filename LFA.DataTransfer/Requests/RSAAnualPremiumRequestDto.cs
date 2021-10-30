using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class RSAAnualPremiumRequestDto
    {
        public Guid Id { get; set; }
        public Guid ContractExtensionId { get; set; }
        public int Year { get; set; }
        public Decimal Value { get; set; }

        public bool RSAAnualPremiumInsertion
        {
            get;
            set;
        }

    }
}
