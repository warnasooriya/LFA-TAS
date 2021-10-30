using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ValidateClaimProcessResponseDto
    {
        public string status { get; set; }
        public string msg { get; set; }
        public Guid statId { get; set; }
        public Guid policyId { get; set; }

    }
}
