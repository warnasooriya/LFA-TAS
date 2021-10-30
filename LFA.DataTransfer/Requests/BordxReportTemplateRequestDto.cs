using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class BordxReportTemplateRequestDto
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual Guid ProductType { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual Guid? UpdateUser { get; set; }
        public virtual List<BordxReportTemplateDetailsRequestDto> BordxReportTemplateDetails { get; set; }
    }
}
