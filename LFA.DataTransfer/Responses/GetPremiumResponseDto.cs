using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class GetPremiumResponseDto
    {
        public string Status { get; set; }
        public string Currency { get; set; }


        public decimal BasicPremium { get; set; }
        public decimal BasicPremiumNRP { get; set; }


        public decimal VariantPremium { get; set; }
        public decimal VariantPremiumNRP { get; set; }

        public decimal TotalPremium { get; set; }
        public decimal TotalPremiumNRP { get; set; }

        public decimal EligibilityPremium { get; set; }
        public decimal Tax { get; set; }

      


    }
}
