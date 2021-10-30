using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyHistoriesResponseDto
    {
        public List<PolicyHistoryResponseDto> Policies
        {
            get;
            set;
        }
    }
}
