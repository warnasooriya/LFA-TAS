using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class CustomerCheckResponseDto
    {
        public Guid CustomerId { get; set; }
        public bool IsBordxConfirmed { get; set; }
        public bool IsPolicyApproved { get; set; }
        public bool HasAccesstoPolicyApproval { get; set; }
    }
}
