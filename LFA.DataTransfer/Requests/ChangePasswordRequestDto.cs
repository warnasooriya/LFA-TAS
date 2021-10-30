using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ChangePasswordRequestDto
    {
        public string oldPassword;
        public string newPassword;
        public Guid userId;

    }
}
