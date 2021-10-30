using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ChangePassswordLinkRequestDto
    {
        public Guid requestId { get; set; }
        public Guid tpaId { get; set; }
    }
}
