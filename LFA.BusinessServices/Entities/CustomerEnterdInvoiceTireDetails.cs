using System;

namespace TAS.Services.Entities
{
    public class CustomerEnterdInvoiceTireDetails
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CustomerEnterdInvoiceId { get; set; }
        public virtual Guid InvoiceCodeTireDetailId { get; set; }
        public virtual string TirePositionCode { get; set; }
        public virtual decimal PurchasedPrice { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual decimal ConversionRate { get; set; }
        
    }
}
