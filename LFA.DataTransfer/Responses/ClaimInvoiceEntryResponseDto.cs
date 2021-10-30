using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class ClaimInvoiceEntryResponseDto
    {
        public Guid Id { get; set; }
        public Guid DealerId { get; set; }
        public DateTime InvoiceReceivedDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal InvoiceAmount { get; set; }
        public Guid CurrencyId { get; set; }
        public Guid CurrencyPeriodId { get; set; }
        public decimal ConversionRate { get; set; }
        public Guid UserAttachmentId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string EntryBy { get; set; }
        public bool IsClaimInvoiceEntryExists { get; set; }
    }
}
