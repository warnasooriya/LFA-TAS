using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxTransferRequestDto
    {
        public int year { get; set; }
        public int month { get; set; }
        public int number { get; set; }
        public Guid userId { get; set; }
        public Guid policyId { get; set; }


    }
}
