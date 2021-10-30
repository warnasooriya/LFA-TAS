using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimInforResponseDto
    {
        public  DateTime LastClaimSubmit { get; set; }
        public int NoOfSubmissions { get; set; }
        public Boolean LastClaimApproved { get; set; }
        public string SubmittedUser { get; set; }
    }
}
