﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class FaultCauseOfFailure
    {
        public virtual Guid Id { get; set; }
        public virtual Guid FaultId { get; set; }
        public virtual string CauseOfFailure { get; set; }
    }
}
