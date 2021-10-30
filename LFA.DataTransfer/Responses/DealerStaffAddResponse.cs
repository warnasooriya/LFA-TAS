using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class DealerStaffAddResponse
    {
        public List<string> userExistList { get; set; }
        public string message { get; set; }
        public bool Assigned { get; set; }
    }
}
