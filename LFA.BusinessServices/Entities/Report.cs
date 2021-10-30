using System;

namespace TAS.Services.Entities
{
    public class Report
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual string ReportSection { get; set; }
        public virtual string ReportDirectory { get; set; }
        public virtual string ReportCode { get; set; }
        public virtual string ReportName { get; set; }
        public virtual string ReportFileName { get; set; }
    }
}
