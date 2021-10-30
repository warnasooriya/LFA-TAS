using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBordxYearlyReopenRequestDto
    {
        public int claimBordxYear { get; set; }
        public Guid claimBordxCountry { get; set; } 
    }
}
