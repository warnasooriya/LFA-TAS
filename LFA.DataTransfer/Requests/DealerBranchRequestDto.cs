using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DealerBranchRequestDto
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public Guid BranchId { get; set; }
    }
}
