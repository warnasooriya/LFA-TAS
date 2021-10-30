using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class GetPoliciesByBordxIdRequestDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public Guid bordxId { get; set; }
    }
}
