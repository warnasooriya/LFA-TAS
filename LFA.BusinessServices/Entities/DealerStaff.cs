﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class DealerStaff
    {
        public virtual Guid Id { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid UserId { get; set; }
    }
}
