using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxReopenRequestDto
    {
        public Guid bordxId { get; set; }
        public Guid userId { get; set; } 

    }
}
