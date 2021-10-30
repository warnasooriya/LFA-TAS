using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class EligibilityCheckResponse
    {
        public decimal premium { get; set; }
        public bool isPercentage { get; set; }
        public string status { get; set; }
    }
}
