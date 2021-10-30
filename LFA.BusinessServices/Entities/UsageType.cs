using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class UsageType
    {
        public virtual int Id { get; set; }
        public virtual string UsageTypeName { get; set; }
        public virtual string UsageTypeCode { get; set; }
    }
}
