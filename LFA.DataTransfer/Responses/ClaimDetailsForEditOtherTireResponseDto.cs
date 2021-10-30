using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimDetailsForEditOtherTireResponseDto
    {
        public Guid ClaimId { get; set; }
        public string Status { get; set; }
        public PolicyDetails PolicyDetails { get; set; }
        public ClaimRequestDetailsResponseDto ClaimDetails { get; set; }
        public ClaimRequestDetailsOtherTireResponseDto ClaimDetailsTire { get; set; }
    }
}
