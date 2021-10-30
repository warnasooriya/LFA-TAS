using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimChequePaymentRequestDto
    {

        public Guid Id { get; set; }
        public string ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public decimal ChequeAmount { get; set; }
        public DateTime EntryDate { get; set; }
        public Guid EntryBy { get; set; }
        public Guid CountryId { get; set; }
        public Guid DealerId { get; set; }
        public DateTime IssuedDate { get; set; }
        public string Comment { get; set; }
        public List<ClaimChequePaymentDetailRequestDto> ClaimChequePaymentDetails { get; set; }
    }
}
