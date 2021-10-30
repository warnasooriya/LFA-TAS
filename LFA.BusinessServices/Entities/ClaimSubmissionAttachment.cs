using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimSubmissionAttachment
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimSubmissionId { get; set; }
        public virtual Guid UserAttachmentId { get; set; }
        public virtual DateTime DateOfAttachment { get; set; }

    }
}
