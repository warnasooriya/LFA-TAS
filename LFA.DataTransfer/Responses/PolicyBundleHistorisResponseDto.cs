using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyBundleHistoriesResponseDto
    {
        public List<PolicyBundleHistoryResponseDto> Policies
        {
            get;
            set;
        }
    }
}
