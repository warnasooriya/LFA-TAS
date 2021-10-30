using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ForgotPasswordRequestDto
    {
        public Guid tpaId;
        public string email;
    }
}
