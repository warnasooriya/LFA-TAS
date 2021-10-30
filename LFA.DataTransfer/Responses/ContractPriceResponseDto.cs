using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ContractPriceResponseDto
    {
        public Guid contractId { get; set; }
        public string dealName { get; set; }
        public string coverType { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
        public double PremiumTotal { get; set; }
        public string PremiumBasedon { get; set; }
        public double price { get; set; }
        public string ExtensionName { get; set; }
        public Guid ExtensionID { get; set; }
        public int Month { get; set; }

    }
}
