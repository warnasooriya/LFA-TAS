using System;

namespace TAS.DataTransfer.Requests
{
    public class ExportPoliciesToExcelByBordxIdRequestDto
    {
        public Guid bordxId { get; set; }
        public Guid userId { get; set; }
        public Guid bordxReportTemplateId { get; set; }
        public Guid dealerId { get; set; }
    }
}
