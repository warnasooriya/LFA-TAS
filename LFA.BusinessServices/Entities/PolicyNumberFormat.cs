using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.Services.Entities
{
    public class PolicyNumberFormat
    {
        public virtual Guid Id { get; set; }
        public virtual String Key { get; set; }
        public virtual int Sequence { get; set; }
        public virtual bool Active { get; set; }
    }
}
