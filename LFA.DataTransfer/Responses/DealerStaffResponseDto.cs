using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class DealerStaffResponseDto
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public Guid UserId { get; set; }
       

        public bool IsDealerStaffExists { get; set; }
     
    }
}
