﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class TPAConfig
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }

    }
}
