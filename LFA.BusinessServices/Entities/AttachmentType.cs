using System;

namespace TAS.Services.Entities
{
    public class AttachmentType
    {
        public virtual Guid Id { get; set; }
        public virtual Guid AttachmentSectionId { get; set; }
        public virtual String AttachmentTypeCode { get; set; }
        public virtual String AttachmentTypeDescription { get; set; }
    }
}