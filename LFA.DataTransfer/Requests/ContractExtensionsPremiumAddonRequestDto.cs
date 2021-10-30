using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ContractExtensionsPremiumAddonRequestDto
    {
        public Guid Id { get; set; }
        public Guid ContractExtensionId { get; set; }
        public Guid PremiumAddonTypeId { get; set; }
        public int Value { get; set; }

        public bool ContractExtensionsPremiumAddonInsertion
        {
            get;
            set;
        }

    }
}
