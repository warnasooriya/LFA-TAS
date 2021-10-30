using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ChangePasswordCustomerRequestDto
    {
        public string oldPassword;
        public string newPassword;
        public Guid userId;
        ////public Guid currentUserId;
        //public Guid currentCustomerId;

       
    }
}
