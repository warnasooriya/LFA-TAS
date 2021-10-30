using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class IncurredErningResponseDto
    {
        public Guid Insurer { get; set; }
        public string InsurerShortName { get; set; }
        public Guid Reinsurer { get; set; }
        public string ReinsurerName { get; set; }
        public string UNRYear { get; set; }
        public Guid Dealer { get; set; }
        public string DealerName { get; set; }
        public string PolicyStatus { get; set; }
        public string WarantyType { get; set; }
        public decimal EarnPercenSum { get; set; }
    }
}
