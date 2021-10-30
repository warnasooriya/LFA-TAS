using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimUpdateRequestDto :ClaimSubmissionRequestDto
    {
        public Guid id { get; set; }
    }
}
