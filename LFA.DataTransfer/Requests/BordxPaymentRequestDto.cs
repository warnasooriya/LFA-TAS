using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxPaymentRequestDto
    {
        
        public Guid Id { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal BordxAmount { get; set; }
        public string RefNo { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public Guid ClaimBordxID { get; set; }

        public bool BordxPaymentInsertion
        {
            get;
            set;
        }
    }
}
