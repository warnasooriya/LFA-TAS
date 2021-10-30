using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimOtherTireUpdateRequestDto : ClaimSubmissionOtherTireRequestDto
    {
        public Guid id { get; set; }
    }
}
