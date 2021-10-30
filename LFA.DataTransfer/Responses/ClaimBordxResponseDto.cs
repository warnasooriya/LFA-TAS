using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimBordxResponseDto
    {
        public Guid Id { get; set; }
        public Guid Insurer { get; set; }
        public string InsurerShortName { get; set; }
        public Guid Reinsurer { get; set; }
        public string ReinsurerName { get; set; }
        public int BordxYear { get; set; }
        public int Bordxmonth { get; set; }
        public string BordxNumber { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsConfirmed { get; set; }
        public Guid UserId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid ProcessedUserId { get; set; }
        public DateTime ProcessedDateTime { get; set; }
        public Guid ConfirmedUserId { get; set; }
        public DateTime ConfirmedDateTime { get; set; }
        public decimal BordxAmount { get; set; }
        public bool IsPaid { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string InvoiceReceivedDate { get; set; }
        public string RefNo { get; set; }
    }
}
