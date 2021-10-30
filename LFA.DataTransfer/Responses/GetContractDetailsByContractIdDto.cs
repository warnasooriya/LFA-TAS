using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class GetContractDetailsByContractIdDto : ContractRequestV2Dto
    {
        public Guid Id { get; set; }
    }
}
