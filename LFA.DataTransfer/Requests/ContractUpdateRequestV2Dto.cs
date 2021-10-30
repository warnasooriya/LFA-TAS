using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ContractUpdateRequestV2Dto : ContractRequestV2Dto
    {
        public Guid contractId { get; set; }
    }
}
