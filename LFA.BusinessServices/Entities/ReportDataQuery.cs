using System;

namespace TAS.Services.Entities
{
    public class ReportDataQuery
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ReportKey { get; set; }
        public virtual string ReportCode { get; set; }
        public virtual string ReportDbConnStr { get; set; }
        public virtual string ReportQuery { get; set; }
        public virtual string ReportDirectory { get; set; }

    }
}
