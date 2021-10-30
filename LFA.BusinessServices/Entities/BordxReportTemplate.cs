using System;

namespace TAS.Services.Entities
{
    public class BordxReportTemplate
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual DateTime? UpdateDateTime { get; set; }
        public virtual Guid? UpdateUser { get; set; }
        public virtual Guid ProductType { get; set; }
    }
}
