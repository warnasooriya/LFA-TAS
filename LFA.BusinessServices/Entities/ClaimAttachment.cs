using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimAttachment
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimId { get; set; }
        public virtual Guid UserAttachmentId { get; set; }
        public virtual DateTime DateOfAttachment{ get; set; }

}
}
