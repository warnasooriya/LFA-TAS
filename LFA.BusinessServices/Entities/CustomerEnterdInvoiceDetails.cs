using System;

namespace TAS.Services.Entities
{
    public class CustomerEnterdInvoiceDetails
    {
        public virtual Guid Id { get; set; }
        public virtual Guid InvoiceCodeId { get; set; }
        public virtual Guid CustomerId { get; set; }
        public virtual string InvoiceCode { get; set; }
        public virtual string InvoiceNumber { get; set; }
        public virtual Guid InvoiceAttachmentId { get; set; }
        public virtual string UsageTypeCode { get; set; }
        public virtual Guid AdditionalDetailsMakeId { get; set; }
        public virtual Guid AdditionalDetailsModelId { get; set; }
        public virtual int AdditionalDetailsModelYear { get; set; }
        public virtual decimal AdditionalDetailsMileage { get; set; }
        public virtual DateTime GeneratedDateTime { get; set; }
    }
}
