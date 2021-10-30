using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class UsersPaymentModeCurrenciesResponseDto
    {
        public object users { get; set; }
        public object paymentModes { get; set; }
        public object currencies { get; set; }
    }
}
