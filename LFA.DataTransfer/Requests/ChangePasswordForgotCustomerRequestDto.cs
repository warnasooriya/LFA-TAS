using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ChangePasswordForgotCustomerRequestDto
    {
        public virtual Guid requestId { get; set; }
        public virtual Guid tpaId { get; set; }
        public virtual string password { get; set; }
        public virtual Guid systemUserId { get; set; }
        public virtual Guid currentCustomerId { get; set; }
    }
}
