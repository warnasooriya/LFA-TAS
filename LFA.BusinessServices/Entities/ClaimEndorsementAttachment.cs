﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class ClaimEndorsementAttachment
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ClaimId { get; set; }
        public virtual Guid UserAttachmentId { get; set; }
    }
}
