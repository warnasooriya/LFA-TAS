using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class AttachmentSection
    {
        public virtual Guid Id { get; set; }
        public virtual String AttachmentScreenName { get; set; }
        public virtual String AttachmentSectionCode { get; set; }

    }
}
