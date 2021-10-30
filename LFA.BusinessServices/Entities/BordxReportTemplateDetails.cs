using System;

namespace TAS.Services.Entities
{
    public class BordxReportTemplateDetails
    {
        public virtual Guid Id { get; set; }
        public virtual Guid BordxReportTemplateId { get; set; }
        public virtual Guid BordxReportColumnsId { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual DateTime? UpdateDateTime { get; set; }
        public virtual Guid? UpdateUser { get; set; }
    }
}
