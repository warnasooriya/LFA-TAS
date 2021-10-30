using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class LoginRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string tpaID { get; set; }
    }
}
