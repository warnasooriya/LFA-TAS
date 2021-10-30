using System;

namespace TAS.DataTransfer.Responses
{
    public class DealerInvoiceReportColumnsResponseDto
    {
        public virtual Guid Id { get; set; }
        public virtual string DisplayName { get; set; }
        public virtual string KeyName { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int Sequance { get; set; }
    }
}
