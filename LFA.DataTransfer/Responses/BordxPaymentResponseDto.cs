using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class BordxPaymentResponseDto
    {
        public Guid Id { get; set; }
        public Guid BordereauID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public decimal BordxAmount { get; set; }
        public string RefNo { get; set; }
        //public string BordxNumber { get; set; }
    }
}
