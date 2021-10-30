using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ReinsureBordxByReinsureIdandYearResponseDto
    {
        public Guid ClaimBordxId { get; set; }
        public string ClaimBordxNo { get; set; }
        public int ClaimBordxMonth { get; set; }
        public string ClaimBordxFromDate { get; set; }
        public string ClaimBordxTodate { get; set; }

        public decimal ClaimBordxValue { get; set; }

        public decimal ClaimBordxPaidAmount { get; set; }
    }
}
