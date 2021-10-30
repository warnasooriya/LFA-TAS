using System;

namespace TAS.Services.Entities
{
    public class ReportParameter
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ReportId { get; set; }
        public virtual string ParamCode { get; set; }
        public virtual string ParamName { get; set; }
        public virtual string ParamType { get; set; }
        public virtual string ClienSideRegex { get; set; }
        public virtual string ServerSideRegex { get; set; }
        public virtual int Sequance { get; set; }
        public virtual string SelectQuery { get; set; }
        public virtual string OnSelectNextParam { get; set; }
        public virtual bool IsServerSideQuery { get; set; }
        public virtual string ServerSideQuery { get; set; }
        public virtual bool IsIncluedeInDateRange { get; set; }
        public virtual bool IsFromField { get; set; }
        public virtual bool IsDependOnPreviousParam { get; set; }
        public virtual int PreviousParamSequance { get; set; }
        public virtual bool IsShowAll { get; set; }
    }
}
