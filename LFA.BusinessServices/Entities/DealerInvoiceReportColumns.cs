using System;

namespace TAS.Services.Entities
{
    public class DealerInvoiceReportColumns
    {
        public virtual Guid Id { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string KeyName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int Sequance { get; set; }
    }
}
