using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
   public class ClaimProcessDataGridResponseDto
    {
        public Guid Id{ get; set; }
        public Guid PolicyId { get; set; }
        public string CommodityType { get; set; }
        public string PolicyNumber { get; set; }
        public string ClaimNumber { get; set; }
        public string ClaimDealer { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal TotalClaimAmount { get; set; }
        public string ClaimAmount  { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Currency { get; set; }
        public Guid ClaimCurrencyId { get; set; }
        public Guid CurrencyPeriodId { get; set; }

    }
}
