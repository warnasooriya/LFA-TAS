using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyBundlesResponseDto
    {
        public List<PolicyBundleResponseDto> Policies
        {
            get;
            set;
        }
    }
}
