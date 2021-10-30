using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimInvoiceEntry
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual DateTime InvoiceReceivedDate { get; set; }
        public virtual DateTime InvoiceDate { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual decimal InvoiceAmount { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual decimal ConversionRate { get; set; }
        public virtual Guid UserAttachmentId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual string EntryBy { get; set; }
        public virtual bool IsConfirm { get; set; }

    }
}
