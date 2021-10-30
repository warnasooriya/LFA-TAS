using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class UserAttachment
    {
        public virtual Guid Id { get; set; }
        public virtual Guid AttachmentSectionId { get; set; }
        public virtual Guid AttachmentTypeId { get; set; }
        public virtual String UserDefinedName { get; set; }
        public virtual String AttachmentFileName { get; set; }
        public virtual decimal AttachmentSizeKB { get; set; }
        public virtual String AttachmentFileType { get; set; }
        public virtual String FileServerReference { get; set; }
        public virtual DateTime UploadedDateTime { get; set; }
        public virtual Guid UserId { get; set; }
    }
}
