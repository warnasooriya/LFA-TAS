using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimsResponseDto
    {
        public List<ClaimResponseDto> Claim
        {
            get;
            set;
        }

        public List<ClaimResponseDto> Claims
        {
            get;
            set;
        }
    }
}
