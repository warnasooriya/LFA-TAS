using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class CustomerLoginResponseDto
    {
        public string JsonWebToken { get; set; }
        public bool IsValid { get; set; }
        public Guid LoggedInUserId { get; set; }
        public Guid LoggedInCustomerId { get; set; }
        public string UserType { get; set; }
        public string LoggedInCustomerName { get; set; }
    }
}
