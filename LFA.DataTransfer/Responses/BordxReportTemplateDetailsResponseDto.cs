using System;

namespace TAS.DataTransfer.Responses
{
    public class BordxReportTemplateDetailsResponseDto
    {
        public virtual Guid Id { get; set; }
        public virtual Guid BordxReportTemplateId { get; set; }
        public virtual Guid BordxReportColumnsId { get; set; }

        public virtual bool IsEnable { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual Guid? UpdateUser { get; set; }
    }
}
