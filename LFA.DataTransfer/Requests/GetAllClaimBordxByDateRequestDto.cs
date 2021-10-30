using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class GetAllClaimBordxByDateRequestDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        public String number { get; set; }
    }
}
