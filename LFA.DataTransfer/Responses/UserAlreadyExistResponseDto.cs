using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class UserAlreadyExistResponseDto
    {
        public bool AlreadyExistUsername { get; set; }
        public bool AlreadyExistEmail { get; set; }
        public string Message { get; set; }
    }
}
