using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimBatchGroupsRespondDto
    {
        public List<ClaimBatchGroupRespondDto> ClaimBatchGroups
        {
            get;
            set;
        }
    }
}
