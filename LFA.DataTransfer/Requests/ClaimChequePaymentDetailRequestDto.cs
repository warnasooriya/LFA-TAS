using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimChequePaymentDetailRequestDto
    {
        ////public Guid Id { get; set; }
        //public Guid ClaimChequePaymentId { get; set; }
        public Guid ClaimBatchGroupId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime BatchDate { get; set; }
        public decimal Amount { get; set; }
        public string GroupName { get; set; }
        //public Guid EntryBy { get; set; }
    }
}
