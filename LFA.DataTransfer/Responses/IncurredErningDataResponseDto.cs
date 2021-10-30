using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class IncurredErningDataResponseDto
    {
        public Guid Insurer { get; set; }
        public string InsurerShortName { get; set; }
        public Guid Reinsurer { get; set; }
        public string ReinsurerName { get; set; }
        public string UNRYear { get; set; }
        public Guid Dealer { get; set; }
        public string DealerName { get; set; }
        public Guid PolicyId { get; set; }
        public string WarantyType { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime MWStartDate { get; set; }

        public DateTime PolicyEndDate { get; set; }
        public decimal Premium { get; set; }
        public decimal EarnPrecen { get; set; }

    }
}
